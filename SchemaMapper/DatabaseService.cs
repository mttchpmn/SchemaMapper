using Dapper;

namespace SchemaMapper;

public class DatabaseService
{
    private readonly IDatabaseConnectionFactory _connectionFactory;

    public DatabaseService(IDatabaseConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<Table>> GetTables()
    {
        var tableNames = await GetTableNames();

        var tableTasks = tableNames.Select(async tableName =>
        {
            var columns = await GetColumnsForTable(tableName);
            var fKeys = await GetForeignKeysForTable(tableName);

            var references = fKeys.Select(y => y.ForeignTableName).ToList();

            return new Table(tableName, columns, fKeys, references);
        }).ToList();

        var tables = await Task.WhenAll(tableTasks);


        return tables.ToList();
    }


    private async Task<List<string>> GetTableNames()
    {
        using var connection = _connectionFactory.GetConnection();

        var sql = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public'";

        var result = await connection.QueryAsync<string>(sql);

        return result.Where(x => !x.StartsWith("pg_")).ToList();
    }

    private async Task<List<Column>> GetColumnsForTable(string tableName)
    {
        await using var connection = _connectionFactory.GetConnection();

        var sql =
            "SELECT column_name, data_type, is_nullable FROM information_schema.columns WHERE table_name = @TableName";

        var result = await connection.QueryAsync<DbColumn>(sql, new {TableName = tableName});
        var primaryKeys = await GetPrimaryKeysForTable(tableName);

        return result.Select(x => new Column(x.column_name, x.data_type, primaryKeys.Contains(x.column_name), x.is_nullable.Equals("YES"))).ToList();
    }

    private async Task<List<string>> GetPrimaryKeysForTable(string tableName)
    {
        await using var connection = _connectionFactory.GetConnection();
        var sql = @"SELECT
                       kcu.column_name
                   FROM
                       information_schema.table_constraints AS tc
                       JOIN information_schema.key_column_usage AS kcu
                         ON tc.constraint_name = kcu.constraint_name
                         AND tc.table_schema = kcu.table_schema
                   WHERE tc.constraint_type = 'PRIMARY KEY' AND tc.table_name=@TableName";

        var primaryKeys = await connection.QueryAsync<string>(sql, new {TableName = tableName});

        return primaryKeys.ToList();
    }

    private async Task<List<ForeignKey>> GetForeignKeysForTable(string tableName)
    {
        await using var connection = _connectionFactory.GetConnection();

        var sql = @"SELECT
                       tc.table_schema, 
                       tc.constraint_name, 
                       tc.table_name, 
                       kcu.column_name, 
                       ccu.table_schema AS foreign_table_schema,
                       ccu.table_name AS foreign_table_name,
                       ccu.column_name AS foreign_column_name 
                   FROM 
                       information_schema.table_constraints AS tc 
                       JOIN information_schema.key_column_usage AS kcu
                         ON tc.constraint_name = kcu.constraint_name
                         AND tc.table_schema = kcu.table_schema
                       JOIN information_schema.constraint_column_usage AS ccu
                         ON ccu.constraint_name = tc.constraint_name
                         AND ccu.table_schema = tc.table_schema
                   WHERE tc.constraint_type = 'FOREIGN KEY' AND tc.table_name=@TableName;";

        var result = await connection.QueryAsync<DbForeignKey>(sql, new {TableName = tableName});

        return result.Select(x => new ForeignKey(x.constraint_name, x.table_name, x.foreign_table_name, x.column_name,
            x.foreign_column_name)).ToList();
    }

    private record DbColumn(string column_name, string data_type, string is_nullable);

    private record DbForeignKey(string table_schema, string constraint_name, string table_name, string column_name,
        string foreign_table_schema, string foreign_table_name, string foreign_column_name);
}

public record Table(string Name, List<Column> Columns, List<ForeignKey> ForeignKeys, List<string> References);

public record Column(string Name, string DataType, bool IsPrimaryKey, bool IsNullable);

public record ForeignKey(string Name, string TableName, string ForeignTableName, string ColumnName,
    string ForeignColumnName);
using System.Text;

namespace SchemaMapper;

public class ErdService : IDiagramService
{
    public string GenerateDiagram(List<Table> tables)
    {
        var result = new StringBuilder();

        foreach (var table in tables)
        {
            result.Append(GenerateEntity(table));
            foreach (var foreignKey in table.ForeignKeys)
            {
                result.Append(GenerateRelationship(foreignKey));
            }
        }

        return result.ToString();
    }

    private string GenerateRelationship(ForeignKey fKey)
        => $"{fKey.TableName.ToUpper()} +--+ {fKey.ForeignTableName.ToUpper()} {{label: \"{fKey.TableName}.{fKey.ColumnName} = {fKey.ForeignTableName}.{fKey.ForeignColumnName}\", color: \"#0832bd\"}}\n";

    private string GenerateEntity(Table table)
    {
        var result = new StringBuilder();
      
        result.Append($"[{table.Name.ToUpper()}]\n");

        foreach (var column in table.Columns)
        {
            var fKeys = table.ForeignKeys.Select(x => x.ColumnName);
            if (fKeys.Contains(column.Name))
            {
                // Mark column as foreign key
                result.Append('+');
            }

            if (column.IsPrimaryKey)
            {
                // Mark column as primary key
                result.Append('*');
            }
         
            result.Append($"{column.Name} {{label: \" {column.DataType}\"}}\n");
        }

        return result.ToString();
    }
}
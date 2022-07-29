using System.Text;

namespace SchemaMapper;

public class ErdService : IDiagramService
{
    private string _foreignKeyColor = "blue";
    private string _primaryKeyColor = "red";
    private string _headerColor = "green";

    public string GenerateDiagram(string? title, List<Table> tables)
    {
        var result = new StringBuilder();

        if (title != null)
        {
            result.Append($"title {{label: \"{title}\"}}\n");
        }

        result.Append($"header {{color: \"{_headerColor}\"}}\n");
        result.Append($"entity {{border-color: \"{_headerColor}\"}}\n");
        result.Append($"relationship {{color: \"{_foreignKeyColor}\"}}\n");

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
        =>
            $"{fKey.TableName.ToUpper()} +--+ {fKey.ForeignTableName.ToUpper()} {{label: \"{fKey.TableName}.{fKey.ColumnName} = {fKey.ForeignTableName}.{fKey.ForeignColumnName}\"}}\n";

    private string GenerateEntity(Table table)
    {
        var result = new StringBuilder();

        result.Append($"[{table.Name.ToUpper()}]\n");

        foreach (var column in table.Columns)
        {
            var spacer = "    ";
            var nullable = column.IsNullable
                ? "nullable"
                : "non-nullable";

            var fKeys = table.ForeignKeys.Select(x => x.ColumnName);
            if (fKeys.Contains(column.Name))
            {
                // Mark column as foreign key
                result.Append(
                    $"+`{column.Name}{spacer}` {{label: \"{column.DataType}, {nullable}\", color: \"{_foreignKeyColor}\"}}\n");
                continue;
            }

            if (column.IsPrimaryKey)
            {
                // Mark column as primary key
                result.Append(
                    $"*`{column.Name}{spacer}` {{label: \"{column.DataType}, {nullable}\", color: \"{_primaryKeyColor}\"}}\n");
                continue;
            }

            result.Append($"`{column.Name}{spacer}` {{label: \"{column.DataType}, {nullable}\"}}\n");
        }

        return result.ToString();
    }
}
using System.Text;

namespace SchemaMapper;

public class MermaidService : IDiagramService
{
   public string GenerateDiagram(string title, List<Table> tables)
   {
      var result = new StringBuilder();
      result.Append("erDiagram\n");

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
      => $"\t{fKey.TableName.ToUpper()} ||--|| {fKey.ForeignTableName.ToUpper()} : \"{fKey.ColumnName} = {fKey.ForeignColumnName}\"\n";

   private string GenerateEntity(Table table)
   {
      var result = new StringBuilder();
      
      result.Append($"\t{table.Name.ToUpper()} {{\n");

      foreach (var column in table.Columns)
      {
         result.Append($"\t\t{column.DataType.Replace(" ", "_")} {column.Name}\n");
      }

      result.Append("\t}\n");

      return result.ToString();
   }
   
}
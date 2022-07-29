namespace SchemaMapper;

public class TableSorter
{
    public List<List<Table>> SortTables(List<Table> tables)
    {
        var result = new List<List<Table>>();

        foreach (var table in tables)
        {
            var index = IsTableReferenced(table, result);
            if (index != null)
            {
                result[index.Value].Add(table);
            }
            else
            {
                result.Add(new List<Table> {table});
            }
        }

        return result;
    }

    private int? IsTableReferenced(Table table, List<List<Table>> tablesList)
    {
        var index = 0;
        foreach (var tableList in tablesList)
        {
            if (TableListContainsTableReference(table, tableList))
            {
                return index;
            }
            index++;
        }

        return null;
    }

    private bool TableListContainsTableReference(Table table, List<Table> tableList)
    {
        var isReferencedByList = tableList.Any(x => x.References.Contains(table.Name));
        var referencesList = table.References.Any(x => tableList.Any(y => y.Name.Equals(x)));

        return isReferencedByList || referencesList;
    }
}
// See https://aka.ms/new-console-template for more information

using System.Threading.Channels;
using SchemaMapper;

Console.WriteLine("SCHEMA MAPPER\n");

var connectionString = "Host=localhost;Port=5432;Database=cdd;User ID=postgres;Enlist=True";
var dbFactory = new DatabaseConnectionFactory(connectionString);
var dbService = new DatabaseService(dbFactory);
var mermaidService = new MermaidService();

var tables = await dbService.GetTables();

var singles = tables.Where(x => !x.References.Any()).ToList();

var sorter = new TableSorter();

var sorted = sorter.SortTables(tables.Where(x => x.References.Any()).ToList());

foreach (var tableList in sorted)
{
    var diagram = mermaidService.GenerateDiagram(tableList);
    var stop = "here";
}

// File.WriteAllText("result.txt", diagram);
Console.WriteLine("DONE.");
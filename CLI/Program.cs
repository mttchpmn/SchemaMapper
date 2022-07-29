// See https://aka.ms/new-console-template for more information

using System.Threading.Channels;
using SchemaMapper;

Console.WriteLine("SCHEMA MAPPER\n");

var connectionString = "Host=localhost;Port=5432;Database=cdd;User ID=postgres;Enlist=True";
var dbFactory = new DatabaseConnectionFactory(connectionString);
var dbService = new DatabaseService(dbFactory);
var mermaidService = new MermaidService();

var title = "FAML CDD Database Schema";
var tables = await dbService.GetTables();

var erd = new ErdService().GenerateDiagram(title, tables);

var path = Directory.GetCurrentDirectory() + "../../../../../result.erd";
File.WriteAllText(path, erd);
Console.WriteLine("DONE.");
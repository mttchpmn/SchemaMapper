using CommandLine;
using RestSharp;
using SchemaMapper;

namespace CLI;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        await CommandLine.Parser.Default.ParseArguments<CliOptions>(args)
            .MapResult(async opts =>
            {
                await Run(opts);

                return 0;
            }, e => Task.FromResult(-1));

        return 0;
    }

    private static async Task Run(CliOptions opts)
    {
        var dbFactory = new DatabaseConnectionFactory(opts.ConnectionString);
        var dbService = new DatabaseService(dbFactory);
        var erdService = new ErdService();

        if (opts.DiagramType is not DiagramType.Erd)
        {
            throw new InvalidOperationException($"{opts.DiagramType.ToString()} is not currently supported");
        }

        Console.WriteLine("Generating schema diagram...");
        
        var tables = await dbService.GetTables();
        
        var diagram = erdService.GenerateDiagram(opts.Title, tables);
        
        var fileName = opts.OutputFileName ?? Directory.GetCurrentDirectory() + "/result.erd";

        await File.WriteAllTextAsync(fileName, diagram);

        Console.WriteLine($"Diagram saved successfully as {fileName}");

        var client = new RestClient("https://kroki.io");
        var request = new RestRequest();
        var payload = new
        {
            diagram_source = diagram,
            diagram_type = "erd",
            output_format = "svg"
        };
        request.AddJsonBody(payload);
        // request.AddFile("file", fileName);
        var response = await client.PostAsync(request);

        await File.WriteAllTextAsync("result.svg", response.Content);

        Console.WriteLine("Diagram saved as result.svg");
    }
}
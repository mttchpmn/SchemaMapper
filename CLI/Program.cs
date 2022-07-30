using CommandLine;

namespace CLI;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        await Parser.Default.ParseArguments<CliOptions>(args)
            .MapResult(async opts =>
            {
                await Run(opts);

                return 0;
            }, e => Task.FromResult(-1));

        return 0;
    }

    private static async Task Run(CliOptions opts)
    {
        var schemaMapper = new SchemaMapper.SchemaMapper(opts.ConnectionString);

        Console.WriteLine("Rendering diagram...");

        await schemaMapper.RenderDiagram(opts.DiagramType, opts.Title, opts.OutputFileName);

        Console.WriteLine("Diagram saved successfully.");
    }
}
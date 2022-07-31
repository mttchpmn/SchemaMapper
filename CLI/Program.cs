using CommandLine;
using Mapper = SchemaMapper.SchemaMapper;

namespace CLI;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        await Parser.Default
            .ParseArguments<GenerateTableListOptions, GenerateTextualRepresentationOptions,
                RenderTextualRepresentationOptions, RenderDiagramOptions, int>(args)
            .MapResult(
                async (GenerateTableListOptions opts) => await GenerateTableList(opts),
                async (GenerateTextualRepresentationOptions opts) => await GenerateTextualRepresentation(opts),
                async (RenderTextualRepresentationOptions opts) => await RenderTextualRepresentation(opts),
                async (RenderDiagramOptions opts) => await RenderDiagram(opts),
                async errs => await Task.FromResult(1)
                );

        return 0;
    }

    private static async Task<int> RenderDiagram(RenderDiagramOptions opts)
    {
        Console.WriteLine("Rendering diagram...");
        var mapper = new Mapper(opts.ConnectionString);
        var tableFilterList = opts.Tables != null
            ? (await File.ReadAllLinesAsync(opts.Tables)).ToList()
            : null;

        await mapper.RenderDiagram(opts.DiagramType, opts.Title, opts.OutputFileName, tableFilterList);

        Console.WriteLine("Done.");

        return 0;
    }

    private static async Task<int> RenderTextualRepresentation(RenderTextualRepresentationOptions opts)
    {
        Console.WriteLine("Rendering textual representation...");
        var mapper = new Mapper(opts.ConnectionString);

        await mapper.RenderTextualRepresentation(opts.DiagramType, opts.OutputFileName, opts.InputFileName);
        
        Console.WriteLine("Done.");
        return 0;
    }
    
    private static async Task<int> GenerateTextualRepresentation(GenerateTextualRepresentationOptions opts)
    {
        Console.WriteLine("Generating textual representation...");
        var mapper = new Mapper(opts.ConnectionString);
        var tableFilterList = opts.Tables != null
            ? (await File.ReadAllLinesAsync(opts.Tables)).ToList()
            : null;

        await mapper.GenerateTextualRepresentation(opts.DiagramType, opts.Title, opts.OutputFileName, tableFilterList);
        
        Console.WriteLine("Done.");
        
        return 0;
    }
    
    private static async Task<int> GenerateTableList(GenerateTableListOptions opts)
    {
        Console.WriteLine("Generating table list...");
        var mapper = new SchemaMapper.SchemaMapper(opts.ConnectionString);

        await mapper.GenerateTableList();

        Console.WriteLine("Saved to disk as `./tables.txt`.");
        
        return 0;
    }
}
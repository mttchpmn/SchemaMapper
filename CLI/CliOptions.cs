using CommandLine;
using SchemaMapper;

namespace CLI;

public class CliOptions
{
    [Option('c', "connection", Required = true, HelpText = "The PostGreSQL connection string")]
    public string ConnectionString { get; set; }
    
    [Option('o', "output", Required = false, HelpText = "The destination filename")]
    public string? OutputFileName { get; set; }
    
    [Option('d', "diagram", Default = DiagramType.Erd, HelpText = "The type of diagram to generate")]
    public DiagramType DiagramType { get; set; }
    
    [Option('t', "title", HelpText = "The title of the diagram")]
    public string? Title { get; set; }
}

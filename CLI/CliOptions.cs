using CommandLine;
using SchemaMapper;

namespace CLI;

public class CliOptions
{
    [Option('c', "connection", Required = true, HelpText = "The PostGreSQL connection string")]
    public string ConnectionString { get; set; }
}

[Verb("generate-table-list", HelpText = "Writes a list of tables to a file on disk.")]
public class GenerateTableListOptions : CliOptions
{
}

[Verb("generate-textual-representation", HelpText = "Creates a textual diagram of database schema and writes to disk.")]
public class GenerateTextualRepresentationOptions : CliOptions
{
    [Option('d', "diagram", Default = DiagramType.Erd, HelpText = "The type of diagram to generate")]
    public DiagramType DiagramType { get; set; }

    [Option('o', "output", Required = false, HelpText = "The destination filename")]
    public string? OutputFileName { get; set; }

    [Option('t', "title", HelpText = "The title of the diagram")]
    public string? Title { get; set; }

    [Option('t', "tables", Required = false,
        HelpText = "A text file containing table names to include in diagram. 1 per line.")]
    public string? Tables { get; set; }
}

[Verb("render-textual-representation",
    HelpText = "Renders a provided textual representation to SVG and writes to disk.")]
public class RenderTextualRepresentationOptions : CliOptions
{
    [Option('d', "diagram", Default = DiagramType.Erd, HelpText = "The type of diagram to generate")]
    public DiagramType DiagramType { get; set; }

    [Option('i', "input", Required = true, HelpText = "The input textual representation file")]
    public string InputFileName { get; set; }

    [Option('o', "output", Required = false, HelpText = "The destination filename")]
    public string? OutputFileName { get; set; }

    [Option('t', "title", HelpText = "The title of the diagram")]
    public string? Title { get; set; }
}

[Verb("render-diagram",
    HelpText = "Generates textual representation of schema, then renders to SVG and writes both to disk.")]
public class RenderDiagramOptions : CliOptions
{
    [Option('d', "diagram", Default = DiagramType.Erd, HelpText = "The type of diagram to generate")]
    public DiagramType DiagramType { get; set; }

    [Option('o', "output", Required = false, HelpText = "The destination filename")]
    public string? OutputFileName { get; set; }

    [Option('t', "title", HelpText = "The title of the diagram")]
    public string? Title { get; set; }

    [Option('f', "filter", Required = false,
        HelpText = "A text file containing table names to include in diagram. 1 per line.")]
    public string? Tables { get; set; }
}
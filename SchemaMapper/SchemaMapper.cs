using System.Diagnostics;

namespace SchemaMapper;

public class SchemaMapper
{
    private readonly DatabaseService _databaseService;
    private readonly ErdService _erdService;
    private readonly MermaidService _mermaidService;
    private readonly RenderService _renderService;

    private readonly string DefaultTitle = "untitled-diagram";

    public SchemaMapper(string connectionString)
    {
        var dbConnectionFactory = new DatabaseConnectionFactory(connectionString);
        _databaseService = new DatabaseService(dbConnectionFactory);

        _erdService = new ErdService();
        _mermaidService = new MermaidService();
        _renderService = new RenderService();
    }

    public async Task RenderDiagram(DiagramType diagramType, string? title, string? filename)
    {
        var textualRepresentation = await GetTextualRepresentation(diagramType, title);
        await RenderTextualRepresentation(diagramType, filename, textualRepresentation);
    }

    public async Task GenerateTextualRepresentation(DiagramType diagramType, string? title, string? fileName)
    {
        var textualRepresentation = await GetTextualRepresentation(diagramType, title);

        var path = GetPath(diagramType, fileName) + "." + GetFileType(diagramType);

        await File.WriteAllTextAsync(path, textualRepresentation);
    }

    private async Task<string> GetTextualRepresentation(DiagramType diagramType, string? title)
    {
        var diagramService = GetDiagramService(diagramType);
        var tables = await _databaseService.GetTables();

        var textualRepresentation = diagramService.GenerateDiagram(title, tables);
        return textualRepresentation;
    }

    public async Task RenderTextualRepresentation(DiagramType diagramType, string? fileName, string textualRepresentation)
    {
        var path = GetPath(diagramType, fileName) + "svg";
        var rendered = await _renderService.RenderDiagram(textualRepresentation, diagramType);

        await File.WriteAllTextAsync(path, rendered);
    }


    private string GetPath(DiagramType diagramType, string? fileName)
    {
        var name = fileName ?? $"{DefaultTitle}.{GetFileType(diagramType)}";
        var path = fileName ?? $"{Directory.GetCurrentDirectory()}/{name}";

        return path;
    }

    private IDiagramService GetDiagramService(DiagramType diagramType)
    {
        return diagramType switch
        {
            DiagramType.Mermaid => _mermaidService,
            DiagramType.Erd => _erdService,
            _ => throw new ArgumentOutOfRangeException(nameof(diagramType), diagramType, null)
        };
    }
    
    private string GetFileType(DiagramType diagramType)
    {
        return diagramType switch
        {
            DiagramType.Mermaid => "txt",
            DiagramType.Erd => "erd",
            _ => throw new ArgumentOutOfRangeException(nameof(diagramType), diagramType, null)
        };
    }
}
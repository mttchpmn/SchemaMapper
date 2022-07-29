namespace SchemaMapper;

public interface IDiagramService
{
    public string GenerateDiagram(string? title, List<Table> tables);
}
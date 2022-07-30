using RestSharp;

namespace SchemaMapper;

public class RenderService
{
    private static readonly string KrokiUrl = "https://kroki.io";
    
    public async Task<string> RenderDiagram(string textualRepresentation, DiagramType diagramType)
    {
        var client = new RestClient(KrokiUrl);
        var request = new RestRequest();
        
        var payload = new
        {
            diagram_source = textualRepresentation,
            diagram_type = diagramType.ToString().ToLower(),
            output_format = "svg"
        };
        request.AddJsonBody(payload);
        
        var response = await client.PostAsync(request);

        if (response.Content == null || !response.IsSuccessful)
        {
            throw new Exception("Error rendering diagram");
        }

        return response.Content;
    }
}
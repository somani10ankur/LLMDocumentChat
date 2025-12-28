using OpenAI.Embeddings;

namespace LLMDocumentChat.Services;

public class EmbeddingService
{
    private readonly EmbeddingClient _client;

    public EmbeddingService(IConfiguration config)
    {
        _client = new EmbeddingClient(
            config["OpenAI:EmbeddingModel"]!, 
            config["OpenAI:ApiKey"]!
        );
    }

    public async Task<float[]> CreateEmbeddingAsync(string text)
    {
        // Call the GenerateEmbeddingAsync to get a single embedding
        var response = await _client.GenerateEmbeddingAsync(text);

        // Use ToFloats() to convert to ReadOnlyMemory<float>
        var vectorMemory = response.Value.ToFloats();

        // Convert to float[] array
        return vectorMemory.ToArray();
    }
}

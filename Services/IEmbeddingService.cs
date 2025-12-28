namespace LLMDocumentChat.Services
{
    public interface IEmbeddingService
    {
        Task<float[]> CreateEmbeddingAsync(string text);
    }
}

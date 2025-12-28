using System;
using System.Linq;
using System.Threading.Tasks;

namespace LLMDocumentChat.Services
{
    public class MockEmbeddingService : IEmbeddingService
    {
        private readonly Random _random = new Random();

        public Task<float[]> CreateEmbeddingAsync(string text)
        {
            // Return a fixed-length vector with random floats (simulate OpenAI embedding)
            var vector = Enumerable.Range(0, 1536) // Use same dimension as OpenAI text-embedding-3-small
                                   .Select(_ => (float)(_random.NextDouble() * 2 - 1)) // random float between -1 and 1
                                   .ToArray();

            return Task.FromResult(vector);
        }
    }
}

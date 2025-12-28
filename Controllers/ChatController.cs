using Microsoft.AspNetCore.Mvc;
using LLMDocumentChat.Services;

namespace LLMDocumentChat.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IEmbeddingService _embeddingService;
    private readonly DocumentService _documentService;

    // In-memory vector store
    private static readonly List<(string Chunk, float[] Vector)> _vectorStore = new();

    public ChatController(IEmbeddingService embeddingService, DocumentService documentService)
    {
        _embeddingService = embeddingService;
        _documentService = documentService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("No file uploaded");

        using var stream = file.OpenReadStream();
        var chunks = _documentService.ExtractTextChunks(stream);

        _vectorStore.Clear();

        foreach (var chunk in chunks)
        {
            var vector = await _embeddingService.CreateEmbeddingAsync(chunk);
            _vectorStore.Add((chunk, vector));
        }

        return Ok(new { Chunks = chunks.Count });
    }

    [HttpPost("query")]
    public async Task<IActionResult> Query([FromBody] string question)
    {
        if (_vectorStore.Count == 0) return BadRequest("No document uploaded");

        var qVector = await _embeddingService.CreateEmbeddingAsync(question);

        // Simple cosine similarity search
        (string Chunk, float Score) best = ("", -1);
        foreach (var item in _vectorStore)
        {
            var score = CosineSimilarity(qVector, item.Vector);
            if (score > best.Score) best = (item.Chunk, score);
        }

        return Ok(new { Answer = best.Chunk });
    }

    private float CosineSimilarity(float[] vecA, float[] vecB)
    {
        float dot = 0, magA = 0, magB = 0;
        for (int i = 0; i < vecA.Length; i++)
        {
            dot += vecA[i] * vecB[i];
            magA += vecA[i] * vecA[i];
            magB += vecB[i] * vecB[i];
        }

        return dot / (float)(Math.Sqrt(magA) * Math.Sqrt(magB) + 1e-10);
    }
}

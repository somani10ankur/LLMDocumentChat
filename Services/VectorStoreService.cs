namespace LLMDocumentChat.Services;

public class VectorStoreService
{
    private readonly List<(float[] Vector, string Text)> _store = new();

    public void Add(float[] vector, string text)
    {
        _store.Add((vector, text));
    }

    public List<string> Search(float[] queryVector, int topK = 3)
    {
        return _store
            .OrderByDescending(x => CosineSimilarity(queryVector, x.Vector))
            .Take(topK)
            .Select(x => x.Text)
            .ToList();
    }

    private static float CosineSimilarity(float[] a, float[] b)
    {
        float dot = 0, magA = 0, magB = 0;
        for (int i = 0; i < a.Length; i++)
        {
            dot += a[i] * b[i];
            magA += a[i] * a[i];
            magB += b[i] * b[i];
        }
        return dot / ((float)(Math.Sqrt(magA) * Math.Sqrt(magB)));
    }
}

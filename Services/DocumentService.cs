using System.Collections.Generic;
using System.IO;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace LLMDocumentChat.Services;

public class DocumentService
{
    public List<string> ExtractTextChunks(Stream pdfStream, int chunkSize = 500)
    {
        using var pdf = PdfDocument.Open(pdfStream);
        var fullText = "";

        foreach (Page page in pdf.GetPages())
        {
            fullText += page.Text + " ";
        }

        // Split into chunks
        var chunks = new List<string>();
        for (int i = 0; i < fullText.Length; i += chunkSize)
        {
            chunks.Add(fullText.Substring(i, Math.Min(chunkSize, fullText.Length - i)));
        }

        return chunks;
    }
}

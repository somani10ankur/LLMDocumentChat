using OpenAI.Chat;

namespace LLMDocumentChat.Services;

public class ChatService
{
    private readonly ChatClient _client;

    public ChatService(IConfiguration config)
    {
        _client = new ChatClient(
            config["OpenAI:ChatModel"],
            config["OpenAI:ApiKey"]
        );
    }

    public async Task<string> AskAsync(string question, List<string> context)
    {
        var systemPrompt =
            "Answer ONLY from the provided context. If not found, say 'Not available in document'.";

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(systemPrompt),
            new UserChatMessage("Context:\n" + string.Join("\n", context)),
            new UserChatMessage("Question:\n" + question)
        };

        var response = await _client.CompleteChatAsync(messages);
        return response.Value.Content[0].Text;
    }
}

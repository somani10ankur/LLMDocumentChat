using LLMDocumentChat.Services;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddSingleton<IEmbeddingService, MockEmbeddingService>(); // Use Mock locally
builder.Services.AddSingleton<DocumentService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();

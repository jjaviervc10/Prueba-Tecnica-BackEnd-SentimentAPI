using SentimentAPI.Endpoints;
using SentimentAPI.Services;
using SentimentAPI.Data;
using SentimentAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Servicios base para APIs
builder.Services.AddEndpointsApiExplorer();

// Dependencias
builder.Services.AddRouting();
builder.Services.AddSingleton<SentimentAnalyzer>();
builder.Services.AddScoped<CommentRepository>();

var app = builder.Build();

//  Middleware global de errores
app.UseMiddleware<GlobalException>();

//  Endpoint raíz (para verificar que la API vive)
app.MapGet("/", () => "SentimentAPI running OK");

// Endpoints del dominio
app.MapComments();

app.Run();

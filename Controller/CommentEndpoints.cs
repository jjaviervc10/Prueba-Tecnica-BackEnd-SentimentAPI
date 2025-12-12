using SentimentAPI.Data;
using SentimentAPI.Models;
using SentimentAPI.Services;

namespace SentimentAPI.Endpoints;

public static class CommentEndpoints
{
    public static void MapComments(this WebApplication app)
    {
        app.MapPost("/api/comments", async (
            CreateCommentRequest request,
            CommentRepository repo,
            SentimentAnalyzer analyzer) =>

        {

           if(string.IsNullOrWhiteSpace(request.ProductId))
                return Results.BadRequest(new { error = "El campo de productId es obligatorio" });

           if(string.IsNullOrWhiteSpace(request.UserId))
                return Results.BadRequest(new { error = "El campo de userId es obligatorio" });


            if (string.IsNullOrWhiteSpace(request.CommentText))
                return Results.BadRequest(new { error = "El campo de ComentText no puede estar vacio" });

            var sentiment = analyzer.Analyze(request.CommentText);
            var result = await repo.CreateAsync(request, sentiment);

            return Results.Created("/api/comments", new
            {
                id = result.id,
                sentiment = result.sentiment
            });
        });

        app.MapGet("/api/comments", async (
            string? productId,
            string? sentiment,
            CommentRepository repo) =>
        {
            var data = await repo.GetAsync(productId, sentiment);
            return Results.Ok(data);
        });

        app.MapGet("/api/sentiment-summary", async (
            CommentRepository repo) =>
        {
            var summary = await repo.GetSummaryAsync();
            return Results.Ok(new
            {
                total_comments = summary.total,
                sentiment_counts = summary.counts
            });
        });
    }
}

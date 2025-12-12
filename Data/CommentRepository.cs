using SentimentAPI.Models;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;

namespace SentimentAPI.Data;

public class CommentRepository
{
    private readonly IConfiguration _config;

    public CommentRepository(IConfiguration config)
    {
        _config = config;
    }

    private SqlConnection CreateConnection()
    {
        return new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        
    }

    public async Task<(int id, string sentiment)> CreateAsync(
        CreateCommentRequest request,
        string sentiment)

    {
      using var connection = CreateConnection();
      await connection.OpenAsync();
        var sql = """
      INSERT INTO Comments (ProductId, UserId, CommentText, Sentiment)
      OUTPUT INSERTED.Id, INSERTED.Sentiment
      VALUES (@ProductId,@UserId, @CommentText, @Sentiment)
    """;

    using var command = new SqlCommand(sql, connection);
    command.Parameters.AddWithValue("@ProductId",request.ProductId);
    command.Parameters.AddWithValue("@UserId", request.UserId);
    command.Parameters.AddWithValue("@CommentText", request.CommentText);
    command.Parameters.AddWithValue("@Sentiment", sentiment);

    try{
    using var reader = await command.ExecuteReaderAsync();
    await reader.ReadAsync();

    return (reader.GetInt32(0),reader.GetString(1));
    }catch(SqlException ex){throw new Exception("Error en DB mientras creamos comment",ex);}
    }

    public async Task<List<CommentDto>>GetAsync(
        string? productId,
        string? sentiment)
    {
        using var connection = CreateConnection();
        await connection.OpenAsync();
        
        
    var sql = """
    SELECT Id, ProductId, UserId, CommentText, Sentiment, CreatedAt
    From Comments
    WHERE (@ProductId IS NULL OR ProductId = @ProductId)
      AND (@Sentiment IS NULL OR Sentiment = @Sentiment)
    ORDER BY CreatedAt DESC
    """;

    using var command = new SqlCommand(sql,connection);
    command.Parameters.AddWithValue("@ProductId",
     string.IsNullOrEmpty(productId)? DBNull.Value : productId);
     command.Parameters.AddWithValue("@Sentiment",
     string.IsNullOrEmpty(sentiment) ? DBNull.Value : sentiment);


    try{
    using var reader = await command.ExecuteReaderAsync();

    var result = new List<CommentDto>();
    while(await reader.ReadAsync())
        {
            result.Add(new CommentDto
            {
            Id = reader.GetInt32(0),
            ProductId = reader.GetString(1),
            UserId= reader.GetString(2),
            CommentText= reader.GetString(3),
            Sentiment = reader.GetString(4),
            CreatedAt = reader.GetDateTime(5) 
            });
        }
       return result; 
    }
    catch(SqlException ex){throw new Exception("Error en DB al obtener comments",ex);}
    }


    public async Task<(int total, Dictionary<string, int>counts)> GetSummaryAsync()
    {
        using var connection = CreateConnection();
        await connection.OpenAsync();

    try{

        var totalCommand = new SqlCommand(
            "SELECT COUNT(*) From Comments ", connection);

         var total = Convert.ToInt32(await totalCommand.ExecuteScalarAsync());
        var counts = new Dictionary<string, int>
        {
            
        {"positivo",0},
        {"negativo",0},
        {"neutral",0}
        };

      var summaryCommand = new SqlCommand(
        "Select Sentiment, Count(*) From Comments Group By Sentiment",
        connection
      );

      using var reader = await summaryCommand.ExecuteReaderAsync();
      while(await reader.ReadAsync())
        {
            counts[reader.GetString(0)]=reader.GetInt32(1);
        }
        return (total,counts);
    }catch(SqlException ex){throw new Exception("Error en DB mientras creamos sentiment summary",ex);}
    }
}

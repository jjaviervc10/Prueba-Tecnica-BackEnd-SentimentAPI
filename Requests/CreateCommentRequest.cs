namespace SentimentAPI.Models;

public class CreateCommentRequest
{
    public string ProductId{get;set;} = string.Empty;
    public string UserId{get;set;} = string.Empty;
    public string CommentText{get;set;}= string.Empty;
}
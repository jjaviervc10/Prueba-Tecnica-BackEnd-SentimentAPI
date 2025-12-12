namespace SentimentAPI.Models;
public class Comment
{
    public int id {get;set;}
    public string ProductId{get;set;} = string.Empty;
    public string UserId{get;set;} = string.Empty;
    public string CommentText{get;set;} = string.Empty;
    public string Sentiment{get;set;}= string.Empty;
    public DateTime CreatedAt {get;set;}

}
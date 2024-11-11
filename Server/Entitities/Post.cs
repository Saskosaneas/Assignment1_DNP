namespace Entities;

public class Post
{
    public string Title { get; set; }
    public string Body { get; set; }
    public int ID { get; set; }
    public int UserID { get; set; }

    public Post(string title, string body,int userID)
    {
        Title = title;
        Body = body;
        UserID = userID;
    }
}
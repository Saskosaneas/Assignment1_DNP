namespace Entities;

public class Comment
{
    public string Body { get; set; }
    public int ID { get; set; }
    public int PostID {get; set;}
    
    public Comment (int id, string body, int postID)
    {
        ID = id;
        Body = body;
        PostID = postID;
     }
}
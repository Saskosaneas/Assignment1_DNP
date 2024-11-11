namespace Entities;

public class Comment
{
    public int UserID { get; set; }
    public string Body { get; set; }
    public int ID { get; set; }
    public int PostID {get; set;}
    
    public Comment (string body, int postID, int userID)
    {
        UserID = userID;
        Body = body;
        PostID = postID;
     }
}
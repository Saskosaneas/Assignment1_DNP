namespace DTOs;

public class PostDto


{
    public string Author { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int UserID { get; set; }
    
    // public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
    public int ID { get; set; }
}
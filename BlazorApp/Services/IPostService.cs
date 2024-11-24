using DTOs;

namespace BlazorApp.Services;

public interface IPostService
{
    public Task<PostDto> CreatePost(PostDto postDto);
    Task<PostDto> UpdatePost(int postID, PostDto postDto);
    Task DeletePost(int PostId);
    Task<PostDto> GetPostById(int PostId);
    Task<IEnumerable<PostDto>> GetPostByTitle(string? title);
    Task<IEnumerable<PostDto>> GetPostByUser(int UserID);
    
    Task<IEnumerable<PostDto>> GetPosts(string? title = null, string? username= null);
    

}
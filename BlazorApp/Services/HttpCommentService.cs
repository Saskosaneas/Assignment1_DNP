using System.Text.Json;
using DTOs;

namespace BlazorApp.Services;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient client;

    public HttpCommentService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<CommentDto> CreateComment(CommentDto commentDto)
    {
        var response = await client.PostAsJsonAsync("api/Comment", commentDto);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<CommentDto>();
        }

        return null;
    }

    public async Task<CommentDto> UpdateComment(int commentId, CommentDto commentDto)
    {
        var response = await client.PutAsJsonAsync($"api/Comment/{commentId}", commentDto);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<CommentDto>();
        }
        
        return null;
    }

    public async Task<CommentDto> GetComment(int commentId)
    {
        var response = await client.GetAsync($"api/Comment/{commentId}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<CommentDto>();
        }
        return null;
    }

    public async Task<IEnumerable<CommentDto>> GetCommentByUser(int userId)
    {
        var response = await client.GetAsync($"api/Comment/GetByUserId/{userId}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<CommentDto>>();
        }

        return new List<CommentDto>();
    }

    public async Task<IEnumerable<CommentDto>> GetCommentPost(int postId)
    {
        var response = await client.GetAsync($"api/Comment/GetByPostId/{postId}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<CommentDto>>();
        }
        
        return new List<CommentDto>();
    }

    public async Task DeleteComment(int commentId)
    {
        var response = await client.DeleteAsync($"api/Comment/{commentId}");

        if (!response.IsSuccessStatusCode)
        {

        }
    }




}
    
    
    
    
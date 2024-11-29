using System.Text.Json;
using BlazorApp.Components.Pages;
using DTOs;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;
    private readonly Dictionary<int, string> posts = new();


    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<PostDto> CreatePost(PostDto request)
    {
        try
        {
            var response = await client.PostAsJsonAsync("/Post", request);

            if (response.IsSuccessStatusCode)
            {
                var createdPost = await response.Content.ReadFromJsonAsync<PostDto>();

                if (createdPost != null)
                {
                    posts[createdPost.ID] = createdPost.Title;
                }

                return createdPost!;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to add post. Status: {response.StatusCode}, Message: {errorMessage}");
            }
        }
        catch (HttpRequestException e)
        {
            throw new Exception("Error while communicating with the server.", e);
        }
        catch (Exception e)
        {
            throw new Exception("An unexpected error occurred while creating the post.", e);
        }
    }

    public async Task<PostDto> UpdatePost(int postID, PostDto postDto)
    {
        var response = await client.PutAsJsonAsync($"api/Post/{postID}", postDto);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<PostDto>();
        }
        
        return null;
    }
    
    public async Task<PostDto> GetPost(int postId)
    {
        var response = await client.GetAsync($"Post/id/{postId}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<PostDto>();
        }

        return null; 
    }
    
    public async Task<IEnumerable<PostDto>> GetPosts(string? title = null, string? username = null)
    {
        var url = "/Post";

        if (!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(username))
        {
            url += $"?title={title}&username={username}";
        }

        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<PostDto>>();
        }

        return new List<PostDto>();
    }
    
    public async Task DeletePost(int postId)
    {
        var response = await client.DeleteAsync($"api/posts/{postId}");

        // You could check if successful, but no return value is needed for DELETE
        if (!response.IsSuccessStatusCode)
        {
            // Handle failure (optional)
        }
    }
    
    public async Task<PostDto> GetPostById(int postId)
    {
        var response = await client.GetAsync($"Post/id/{postId}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<PostDto>();
        }
        
        return null; 
    }
    
    public async Task<IEnumerable<PostDto>> GetPostByTitle(string? title)
    {
        var url = string.IsNullOrEmpty(title) 
            ? "api/posts" 
            : $"api/posts/title/{title}";

        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<PostDto>>();
        }

        return new List<PostDto>(); 
    }
    
    public async Task<IEnumerable<PostDto>> GetPostByUser(int userId)
    {
        var response = await client.GetAsync($"api/posts/user/{userId}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<PostDto>>();
        }

  
        return new List<PostDto>(); 
    }

    
    
}

using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IpostReporsitory
{
    private readonly string filepath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(filepath))
        {
            File.WriteAllText(filepath, "[]");
        }
    }
    
    private async Task<List<Post>> GetPosts()
    {
        string postsAsJson = await File.ReadAllTextAsync(filepath);
        return JsonSerializer.Deserialize<List<Post>>(postsAsJson);
    }

    private async Task WritePosts(List<Post> posts)
    {
        string postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filepath, postsAsJson);
    }
    
    public async Task<Post> AddAsync(Post post)
    {
        var posts = await GetPosts();
        int maxId = posts.Count > 0 ? posts.Max(x => x.ID) : 0;
        post.ID = maxId + 1;
        posts.Add(post);
        await WritePosts(posts);
        return post;
    }
    
    public async Task UpdateAsync(Post post)
    {
        var posts = await GetPosts();
        Post postToUpdate = posts.FirstOrDefault(x => x.ID == post.ID);
        posts.Remove(postToUpdate);
        posts.Add(post);
        await WritePosts(posts);
    }
    
    public async Task DeleteAsync(int id)
    {
        var posts = await GetPosts();
        Post postToDelete = posts.FirstOrDefault(x => x.ID == id);
        posts.Remove(postToDelete);
        await WritePosts(posts);
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        var posts = await GetPosts();
        Post postToReturn = posts.FirstOrDefault(x => x.ID == id);
        await WritePosts(posts);
        return postToReturn;
    }
    
    
    public IQueryable<Post> GetMany()
    {
        string commentsAsJson = File.ReadAllTextAsync(filepath).Result;
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(commentsAsJson)!;
        return posts.AsQueryable();
    }
    
}
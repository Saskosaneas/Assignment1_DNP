using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : IcommentRepository
{
    private readonly string filepath = "comments.json";

    public CommentFileRepository()
    {
        if (!File.Exists(filepath))
        {
            File.WriteAllText(filepath, "[]");
        }
    }

    private async Task<List<Comment>> GetComments()
    {
        string commentsAsJson = await File.ReadAllTextAsync(filepath);
        return JsonSerializer.Deserialize<List<Comment>>(commentsAsJson);
    }

    private async Task WriteComments(List<Comment> comments)
    {
        string commentsAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filepath, commentsAsJson);
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        var comments = await GetComments();
        int maxId = comments.Count > 0 ? comments.Max(x => x.ID) : 1;
        comment.ID = maxId + 1;
        comments.Add(comment);
        await WriteComments(comments);
        return comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        var comments = await GetComments();
        Comment commentToUpdate = comments.FirstOrDefault(x => x.ID == comment.ID);
        comments.Remove(commentToUpdate);
        comments.Add(comment);
        await WriteComments(comments);
    }

    public async Task DeleteAsync(int id)
    {
        var comments = await GetComments();
        Comment commentToDelete = comments.FirstOrDefault(x => x.ID == id);
        comments.Remove(commentToDelete);
        await WriteComments(comments);
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
        var comments = await GetComments();
        Comment commentToReturn = comments.FirstOrDefault(x => x.ID == id);
        await WriteComments(comments);
        return commentToReturn;
    }
    
    
    public IQueryable<Comment> getMany()
    {
        string commentsAsJson = File.ReadAllTextAsync(filepath).Result;
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        return comments.AsQueryable();
    }
    
    

}
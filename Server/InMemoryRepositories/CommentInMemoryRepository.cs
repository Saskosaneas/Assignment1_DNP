using Entities;
using RepositoryContracts;

public class CommentInMemoryRepository : IcommentRepository
{
    private List<Comment> comments;
    
    public CommentInMemoryRepository()
    {
        comments = new List<Comment>();
    }
    
    public Task<Comment> AddAsync(Comment comment)
    {
        comment.ID = comments.Any()
            ? comments.Max(p => p.ID) + 1
            : 1;
        comments.Add(comment);
        return Task.FromResult(comment); 
    }
    
    public Task UpdateAsync(Comment comment)
    {
        Comment? existingComment = comments.SingleOrDefault(p => p.ID == comment.ID);
        if (existingComment == null)
        {
            throw new InvalidOperationException(
                $"Comment with ID'{comment.ID}'not found");
        }
        comments.Remove(existingComment);
        comments.Add(comment);
        return Task.CompletedTask;
    }
    
    public Task DeleteAsync(int ID)
    {
        Comment? commentToRemove = comments.SingleOrDefault(p => p.ID == ID);
        if (commentToRemove is null)
        {
            throw new InvalidOperationException($"Comment with ID '{ID}' not found");
        }

        comments.Remove(commentToRemove);
        return Task.CompletedTask;
    }

    public Task<Comment> GetSingleAsync(int ID)
    {
        Comment? comment = comments.SingleOrDefault(p => p.ID == ID);
        if (comments is null)
        {
            throw new InvalidOperationException( $"Comment with ID '{ID}' not found");
        }
        return Task.FromResult(comment);
    }

    
    public IQueryable<Comment> getMany()
    {
        return comments.AsQueryable();
    }
    
    
}
using DTOs;

namespace BlazorApp.Services;

public interface ICommentService
{
    Task<CommentDto> CreateComment(CommentDto commentDto);
    Task<CommentDto> UpdateComment(int commentID, CommentDto commentDto);
    Task<CommentDto> GetComment(int commentId);
    Task<IEnumerable<CommentDto>> GetCommentByUser(int userId);
    Task<IEnumerable<CommentDto>> GetCommentPost(int postId);
    Task DeleteComment(int commentId);
}
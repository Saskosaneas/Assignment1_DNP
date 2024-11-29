using DTOs;
using Entities;
using FileRepositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using RepositoryContracts;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]


public class CommentController
{
    private readonly IcommentRepository commentRepository;
    private readonly IuserRepository userRepository;

    public CommentController(IcommentRepository commentRepository, IuserRepository userRepository)
    {
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
    }

    [HttpPost]
    public async Task<IResult> AddComment([FromBody] CommentDto req,
        [FromServices] IcommentRepository commentRepository)
    {
        Comment comment = new Comment(req.Body,req.postID,req.UserID);
        Comment created = await commentRepository.AddAsync(comment);

        CommentDto dto = new()
        {
            Body = created.Body,
            postID = created.PostID,
            UserID = created.UserID
        };
        return Results.Created($"comments/{created.ID}", dto);    }

    [HttpPut("{id}")]
    public async Task<IResult> ReplaceComment([FromRoute] int id,
        [FromBody] CommentDto req,
        [FromServices] IcommentRepository commentRepository)
    {
        Comment existingComment = await commentRepository.GetSingleAsync(id);
        existingComment.Body = req.Body;
        existingComment.PostID = req.postID;
        existingComment.UserID = req.UserID;
        await commentRepository.UpdateAsync(existingComment);
        return Results.Ok();
    }

    [HttpGet("user/{id}")]
    public async Task<IResult> GetCommentsByUser([FromRoute] int id)
    {
        var comments = commentRepository.getMany();
        comments = comments.Where(c => c.UserID == id);
        return Results.Ok(comments);
    }

    [HttpGet("post/{postID}")]
    public async Task<IResult> GetCommentsByPost([FromRoute] int postID)
    {
        var comments = commentRepository.getMany().Where(c => c.PostID == postID).ToList();
        
        var users = userRepository.getMany().ToList();

        var commentDtos = comments.Select(c => new CommentDto
        {
            ID = c.ID,
            Body = c.Body,
            postID = c.PostID,
            UserID = c.UserID,
            Author = users.FirstOrDefault(u => u.ID == c.UserID)?.Username ?? "Unknown" // Fetch author username
        }).ToList();

        return Results.Ok(commentDtos);
    }

    [HttpDelete("{id:int}")]
    public async Task<IResult> DeleteComment([FromRoute] int id)
    {
        await commentRepository.DeleteAsync(id);
        return Results.NoContent();
    }
    
}


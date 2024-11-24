using DTOs;
using Entities;
using FileRepositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]



public class PostController : ControllerBase
{
    private readonly IpostReporsitory postReporsitory;

    public PostController(IpostReporsitory postReporsitory)
    {
        this.postReporsitory = postReporsitory;
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> CreatePost([FromBody] PostDto req,
        [FromServices] IpostReporsitory postReporsitory)
    {
        Post post = new Post(req.Title, req.Body, req.UserID);
        Post created = await postReporsitory.AddAsync(post);
        PostDto dto = new()
        {
            Title = created.Title,
            Body = created.Body,
            UserID = created.UserID,
        };
        return Created($"posts/{created.ID}", dto);
    }

    [HttpPut("{id}")]
    public async Task<IResult> ReplacePost([FromRoute] int id,
        [FromBody] PostDto req, [FromServices] IpostReporsitory postReporsitory)
    {
        Post existingPost = await postReporsitory.GetSingleAsync(id);
        existingPost.Title = req.Title;
        existingPost.Body = req.Body;
        existingPost.UserID = req.UserID;
        
        await postReporsitory.UpdateAsync(existingPost);
        return Results.Ok();
    }

    [HttpGet("id/{id}")]
    public async Task<IResult> GetPost([FromRoute] int id, [FromServices] IuserRepository userRepository)
    {
        // Fetch the post
        var post = await postReporsitory.GetSingleAsync(id);

        // Check if the post exists
        if (post == null)
        {
            return Results.NotFound($"Post with ID {id} not found.");
        }

        // Fetch the user (author) associated with the post
        var user = await userRepository.GetSingleAsync(post.UserID);

        // Check if the user exists
        if (user == null)
        {
            return Results.NotFound($"User with ID {post.UserID} not found.");
        }

        // Map the post and user to a PostDto
        var postDto = new PostDto
        {
            ID = post.ID,
            Title = post.Title,
            Body = post.Body,
            UserID = post.UserID,
            Author = user.Username // Assign the username as the author
        };

        // Return the DTO
        return Results.Ok(postDto);
    }
    [HttpGet]
    public async Task<IResult> GetPostByTitle([FromQuery] string? title)
    {
        var posts = postReporsitory.GetMany();
        if (!string.IsNullOrEmpty(title))
        {
            posts = posts.Where(
                t => t.Title.ToLower().Contains(title.ToLower()));
        }

        return Results.Ok(posts);
    }
    
    [HttpPost("user/{userID}")]
    public async Task<IResult> GetPostByUser ([FromRoute] int? userID)
    {
        var posts = postReporsitory.GetMany();
        posts = posts.Where(t => t.UserID == userID);
        return Results.Ok(posts);
    }

    [HttpDelete("{id:int}")]
    public async Task<IResult> DeletePost([FromRoute] int id)
    {
        await postReporsitory.DeleteAsync(id);
            return Results.NoContent();
    }
    
    
    
}
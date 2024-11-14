using DTOs;
using Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApplication1.Controllers;
[ApiController]
[Route("[controller]")]

public class UserController:ControllerBase
{
    private readonly IuserRepository userRepository;

    public UserController(IuserRepository userRepository)
    {
        this.userRepository = userRepository;
    }
    
    
[HttpPost]
public async Task<ActionResult<UserDto>> AddUser([FromBody] UserDto req, [FromServices] IuserRepository userRepository)
{
    User user = new User(req.Username, req.Password);
    User created = await userRepository.AddAsync(user);
    UserDto userDto = new UserDto
    {
        Username = created.Username,
        Password = created.Password
    };
        
    return Created($"users/{created.ID}", userDto);
}

[HttpGet("{id}")]
public async Task<IResult> GetUser([FromRoute] int id)
{
    User user = await userRepository.GetSingleAsync(id);
    if (user == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(user);
}

[HttpPut("{id}")]
public async Task<IResult> ReplaceUser([FromRoute] int id,
    [FromBody] UserDto req, [FromServices] IuserRepository userRepository)
{
    User existingUser = await userRepository.GetSingleAsync(id);
    if (existingUser == null)
    {
        return Results.NotFound();
    }

    existingUser.Username = req.Username;
    existingUser.Password = req.Password;

    await userRepository.UpdateAsync(existingUser);
    
    return Results.Ok(existingUser);
}

[HttpDelete("{id:int}")]
public async Task<IResult> DeleteUser([FromRoute] int id)
{
    await userRepository.DeleteAsync(id);
    return Results.NoContent();
}

[HttpGet]
public async Task<IResult> GetMany([FromQuery] string? userUsername)
{
    var users = userRepository.getMany();
    if (!String.IsNullOrEmpty(userUsername))
        users = users.Where(u => u.Username.ToLower().Contains(userUsername.ToLower()));
    return Results.Ok(users);
}

}
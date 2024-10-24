using DTOs;
using Entities;
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
        UserDto = new()
    {
        Username = created.Username,
        Password = created.Password
    };
        
    return Created($"users/{created.ID}", DTOs.UserDto);
}


}
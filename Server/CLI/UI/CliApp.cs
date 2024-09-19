using Entities;
using InMemoryRepositories;
using RepositoryContracts;
namespace CLI.UI;

public class CliApp
{
    private readonly IuserRepository _userRepository;
    private readonly IcommentRepository _commentRepository;
    private readonly IpostReporsitory _postRepository;

    public CliApp(IuserRepository userRepository, IcommentRepository commentRepository, IpostReporsitory postRepository)
    {
        _userRepository = userRepository;
        _commentRepository = commentRepository;
        _postRepository = postRepository;
    }

    public async Task StartAsync()
    {   
        Console.WriteLine("fdafadafa........");
    }
}


using Entities;
using FileRepositories;
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
        string? input;
        do
        {
            Console.WriteLine("Forum menu: Select an action");
            Console.WriteLine("1: Create new user");
            Console.WriteLine("2: View all users");
            Console.WriteLine("3: Create new post");
            Console.WriteLine("4: View all posts");
            Console.WriteLine("5: Add comments to a post");
            Console.WriteLine("6: View specific post");
            Console.WriteLine("7: Exit");
            
            input = Console.ReadLine();

            switch (input)
            {
             case  "1":
                 await CreateUserAsync();
                 break;
             case  "2":
                await ViewAllUsersAsync();
                 break;
             case "3":
                await CreatePostAsync();
                break;
             case "4":
                await ViewAllPostsAsync();
                break;
             case "5":
                await AddComentAsync();
                break;
             case "6":
                await FindSpecificPostAsync();
                break;
             
            }

        } while (input != "7");
    }
    private async Task CreateUserAsync()
    {
        Console.WriteLine("Enter username: ");
        string? username = Console.ReadLine();

        Console.WriteLine("Enter password: ");
        string? password = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Invalid input. Username and password cannot be empty.");
            return;
        }
        await _userRepository.AddAsync(new User(username,password));
        Console.WriteLine("User created successfully");
    }
    
    private async Task ViewAllUsersAsync()
    {
        var users = _userRepository.getMany();
        Console.WriteLine("All users");
        foreach (var user in users)
        Console.WriteLine($"ID: {user.ID}, Name:{user.Username}");
    }
    
    
    private async Task CreatePostAsync()
        {
            Console.WriteLine("Give the post a title");
            string? title = Console.ReadLine();
            
            Console.WriteLine("Body:");
            string? body = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(body))
                {
                    Console.WriteLine("Invalid input. Title and body cannot be empty.");
                    return;
                }
                Console.WriteLine("Enter a user ID");
                int userID = Convert.ToInt32(Console.ReadLine());
                
            await _postRepository.AddAsync(new Post(title, body,userID));
                Console.WriteLine("Post created successfully.");
         }
        
    private async Task ViewAllPostsAsync()
        {
            var posts = _postRepository.GetMany();
            Console.WriteLine("All posts");
            foreach (var post in posts)
            Console.WriteLine($"Title: {post.Title}, Body:{post.Body}, UserID:{post.UserID}, postID:{post.ID}");
        }
        
    private async Task AddComentAsync()
            {
                Console.WriteLine("Which post you want to comment ( give ID of the post )?");
                int postID_comment = Convert.ToInt32(Console.ReadLine());
                
                Console.WriteLine("Write your comment");
                string? body = Console.ReadLine();
                
                await _commentRepository.AddAsync(new Comment(1, body,postID_comment));
                Console.WriteLine("Comment added successfully.");
                }
            
    private async Task FindSpecificPostAsync()
        {
            var posts = _postRepository.GetMany();
            Console.WriteLine("What post are you looking for? (give ID)");
             int postID_search = Convert.ToInt32(Console.ReadLine()); 
            
            Post? post = await _postRepository.GetSingleAsync(postID_search);
            if (post == null)
            {
                Console.WriteLine("Post does not exist");
                return;
            }
            
            Console.WriteLine($"Title: {post.Title}");
            Console.WriteLine($"Body: {post.Body}");
            Console.WriteLine($"User ID: {post.UserID}");
            
}
        }


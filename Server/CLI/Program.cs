using CLI.UI;
using InMemoryRepositories;
using RepositoryContracts;

Console.WriteLine("Starting CLI app...");
IuserRepository userRepository = new UserInMemoryRepository();
IcommentRepository commentRepository = new CommentInMemoryRepository();
IpostReporsitory postReporsitory = new PostInMemoryRepository();

CliApp cliApp = new CliApp(userRepository, commentRepository, postReporsitory);
await cliApp.StartAsync();
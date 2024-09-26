using CLI.UI;
using FileRepositories;
using RepositoryContracts;

Console.WriteLine("Starting CLI app...");
IuserRepository userRepository = new UserFileRepository();
IcommentRepository commentRepository = new CommentFileRepository();
IpostReporsitory postReporsitory = new PostFileRepository   ();

CliApp cliApp = new CliApp(userRepository, commentRepository, postReporsitory);
await cliApp.StartAsync();
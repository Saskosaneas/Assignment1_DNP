using System.Text.Json;
using Entities;
using RepositoryContracts;
namespace FileRepositories;

public class UserFileRepository : IuserRepository
{
    private readonly string filepath = "users.json";

    public UserFileRepository()
    {
        if (!File.Exists(filepath))
        {
            File.WriteAllText(filepath, "[]");
        }
    }
    
    private async Task<List<User>> GetUsers()
    {
        string usersAsJson = await File.ReadAllTextAsync(filepath);
        return JsonSerializer.Deserialize<List<User>>(usersAsJson);
    }

    private async Task WriteUsers(List<User> users)
    {
        string usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filepath, usersAsJson);
    }
    
    public async Task<User> AddAsync(User user)
    {
        var users = await GetUsers();
        int maxId = users.Count > 0 ? users.Max(x => x.ID) : 0;
        user.ID = maxId + 1;
        users.Add(user);
        await WriteUsers(users);
        return user;
    }
    
    public async Task UpdateAsync(User user)
    {
        var users = await GetUsers();
        User userToUpdate = users.FirstOrDefault(x => x.ID == user.ID);
        users.Remove(userToUpdate);
        users.Add(user);
        await WriteUsers(users);
    }
    
    public async Task DeleteAsync(int id)
    {
        var users = await GetUsers();
        User userToDelete = users.FirstOrDefault(x => x.ID == id);
        users.Remove(userToDelete);
        await WriteUsers(users);
    }

    public async Task<User> GetSingleAsync(int id)
    {
        var users = await GetUsers();
        User userToReturn = users.FirstOrDefault(x => x.ID == id);
        await WriteUsers(users);
        return userToReturn;
    }
    
    
    public IQueryable<User> getMany()
    {
        string commentsAsJson = File.ReadAllTextAsync(filepath).Result;
        List<User> users = JsonSerializer.Deserialize<List<User>>(commentsAsJson)!;
        return users.AsQueryable();
    }
}
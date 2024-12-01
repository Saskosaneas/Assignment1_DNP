using System.Text;
using System.Text.Json;
using BlazorApp.Components.Pages;
using DTOs;
namespace BlazorApp.Services;

public class HttpUserService : IUserService
{
    private readonly HttpClient client;
    private readonly Dictionary<int, string> usernames = new();

    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<string> GetUserAsync(int userId)
    {
        if (usernames.ContainsKey(userId))
        {
            return usernames[userId]; // Return cached username
        }

        try
        {
            var user = await client.GetFromJsonAsync<UserDto>($"api/User/{userId}");
            string username = user?.Username ?? "Unknown";

            usernames[userId] = username;

            return username;
        }
        catch (HttpRequestException e)
        {
            throw new Exception($"Error fetching user with ID {userId}.", e);
        }
        catch (Exception e)
        {
            throw new Exception("An unexpected error occurred.", e);
        }
    }

    public async Task<UserDto> AddUserAsync(UserDto request)
    {
        try
        {
            var response = await client.PostAsJsonAsync("/User", request);

            if (response.IsSuccessStatusCode)
            {
                var createdUser = await response.Content.ReadFromJsonAsync<UserDto>();

                if (createdUser != null)
                {
                    usernames[createdUser.ID] = createdUser.Username;
                }

                return createdUser!;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to add user. Status: {response.StatusCode}, Message: {errorMessage}");
            }
        }
        catch (HttpRequestException e)
        {
            throw new Exception("Error while communicating with the server.", e);
        }
        catch (Exception e)
        {
            throw new Exception("An unexpected error occurred while adding the user.", e);
        }
    }

    public async Task DeleteUserAsync(int id)
    {
        var response = await client.DeleteAsync($"api/User/{id}");

        if (response.IsSuccessStatusCode)
        { 
            usernames.Remove(id);
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to delete user. Status: {response.StatusCode}, Message: {errorMessage}");
        }
    }
    
    public async Task<IEnumerable<UserDto>> GetAllUsers(string? username = null)
    {
        var query = string.IsNullOrEmpty(username) ? "" : $"?username={Uri.EscapeDataString(username)}";

        var response = await client.GetAsync($"User{query}");
    
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var users = JsonSerializer.Deserialize<IEnumerable<UserDto>>(jsonResponse, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return users ?? Enumerable.Empty<UserDto>();
    }
    
    public async Task UpdateUserAsync(int id, UserDto request)
    {
        var jsonContent = new StringContent(
            JsonSerializer.Serialize(request), 
            Encoding.UTF8, 
            "application/json"
        );

        var response = await client.PutAsync($"api/User/{id}", jsonContent);

        response.EnsureSuccessStatusCode();
    }
    
    public async Task<UserDto> GetUserByUsername(string username)
    {
        var response = await client.GetAsync($"/User?username={username}");
        var responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseContent);  // Log the raw JSON to the console

        if (response.IsSuccessStatusCode)
        {
            try
            {
                // Deserialize into a List<UserDto>
                var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();

                // Find the user by username
                return users?.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing JSON: {ex.Message}");
                throw;
            }
        }
        return null;
    }

    
    

}
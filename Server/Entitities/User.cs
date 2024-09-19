namespace Entities;

public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public int ID { get; set; }
    
    public User(int id, string username, string password)
    {
        ID = id;
        Username = username;
        Password = password;
      
        }
    
}
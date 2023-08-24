using Backend.Enums;
using Backend.Model;

namespace Backend.Services;

public interface IUserService
{
    public Task<bool> Login(string username, string password);
    public Task<bool> Register(string username, string password);
    public Task<Dictionary<string, UserLevel>> GetAllUsers();
    public Task<bool> UpdateUser(User user);
    public Task<User> GetUser(string username);

    public string HashPassword(string password);
}
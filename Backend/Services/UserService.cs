using System.Security.Cryptography;
using Backend.Enums;
using Backend.Exception;
using Backend.Model;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class UserService : IUserService
{
    private GoalContext _dbContext { get; }

    public UserService(GoalContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Boolean> Login(string username, string password)
    {
        User? user;
        try
        {
            user = await _dbContext.GoalUsers.FirstAsync(u => u.UserName.Equals(username));
        }
        catch (System.Exception e)
        {

            return false;
        }

        return !user.Equals(null) && user.CheckPassword(HashPassword(password));
    }

    public async Task<Boolean> Register(string username, string password)
    {
        User? user = await _dbContext.GoalUsers.FirstOrDefaultAsync(user => user.UserName.Equals(username));
        if (user != null)
        {
            return false;
        }

        try
        {
            _dbContext.GoalUsers.Add(new User
            {
                UserName = username,
                Password = HashPassword(password),
                UserLevel = 0
            });
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (System.Exception e)
        {
            return false;
        }
    }

    public async Task<Dictionary<string, UserLevel>> GetAllUsers()
    {
        Dictionary<string, UserLevel> usersWithoutPass = new Dictionary<string, UserLevel>();
        var userList = await _dbContext.GoalUsers.ToListAsync();
        foreach (var user in userList)
        {
            usersWithoutPass.Add(user.UserName, user.UserLevel);
        }

        return usersWithoutPass;
    }

    public async Task<Boolean> UpdateUser(User user)
    {
        var userToUpdate = await _dbContext.GoalUsers.FirstOrDefaultAsync(u => u.UserName.Equals(user.UserName));
        if (userToUpdate != null)
        {
            userToUpdate.UserLevel = user.UserLevel;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        else return false;

    }

    public async Task<User> GetUser(string username)
    {
        var user = await _dbContext.GoalUsers.FirstOrDefaultAsync(u => u.UserName.Equals(username));
        if (user != null)
        {
            return user;
        }
        throw new NotFoundException("User not found");
    }

    private string HashPassword(string pass)
    {
        byte[] salt = { 187, 69, 193, 241, 190, 187, 23, 10, 114, 164, 239, 80, 79, 38, 7, 93 };
        Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: pass!,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

        return hashed;
    }
}


using Backend.Enums;
using Backend.Exception;
using Backend.Model;
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

        return !user.Equals(null) && user.CheckPassword(password);
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
                Password = password.GetHashCode().ToString(),
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
}


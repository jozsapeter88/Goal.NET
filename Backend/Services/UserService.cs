﻿using Backend.Model;
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
                Password = password
            });
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (System.Exception e)
        {
            return false;
        }
    }
}
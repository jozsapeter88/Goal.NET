using Backend.Model;

namespace Backend.Services;

public class UserService : IUserService
{
    private GoalContext _dbContext { get; }
    public UserService(GoalContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public bool Login(string username, string password)
    {
        User? user;
        try
        {
            user = _dbContext.GoalUsers.First(u => u.UserName.Equals(username));
        }
        catch (System.Exception e)
        {
            
            return false;
        }
         
        return !user.Equals(null) && user.CheckPassword(password);
    }

    public bool Register(string username, string password)
    {
        User? user = _dbContext.GoalUsers.FirstOrDefault(user => user.UserName.Equals(username));
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
            _dbContext.SaveChangesAsync();
            return true;
        }
        catch (System.Exception e)
        {
            return false;
        }
    }
}
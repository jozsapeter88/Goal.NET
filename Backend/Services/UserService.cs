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
        var user = _dbContext.GoalUsers.Where(user => user.UserName.Equals(username)).First();
        if (!user.Equals(null))
        {
            return user.CheckPassword(password); 
        }

        return false;
    }

    public bool Register(string username, string password)
    {
        var user = _dbContext.GoalUsers.Where(user => user.UserName.Equals(username)).First();
        if (!user.Equals(null))
        {
            try
            {
                _dbContext.GoalUsers.Add(new User
                {
                    UserName = username, 
                    Password = password
                });
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        return false;
    }
}
using Backend.Enums;
using Backend.Model;
using Backend.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

namespace GoalTest;

[TestFixture]
public class UserServiceTest
{
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<GoalContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _dbContext = new GoalContext(options);
        _userService = new UserService(_dbContext);

        _dbContext.GoalUsers.AddRange(
            new User { UserName = "TestUser1", Password = HashPassword("1234"), UserLevel = UserLevel.User },
            new User { UserName = "TestUser2", Password = HashPassword("1234"), UserLevel = UserLevel.Operator },
            new User { UserName = "TestUser3", Password = HashPassword("1234"), UserLevel = UserLevel.Admin }
        );
        _dbContext.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    private IUserService _userService;
    private GoalContext _dbContext;

    [Test]
    public async Task RegisterUser()
    {
        var result = await _userService.Register("TestUser4", HashPassword("1234"));
        Assert.That(result);
    }

    [Test]
    public async Task RegisterUserException()
    {
        var result = await _userService.Register("TestUser1", HashPassword("1234"));
        Assert.That(!result);
    }

    [Test]
    public async Task GetUser()
    {
        var username = "TestUser1";
        var result = await _userService.GetUser(username);
        var expected = await _dbContext.GoalUsers.FirstOrDefaultAsync(u => u.UserName == username);
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public async Task GetAllUsers()
    {
        var result = await _userService.GetAllUsers();
        var expected = new Dictionary<string, UserLevel>();
        var expectedUsers = await _dbContext.GoalUsers.ToListAsync();
        foreach (var expectedUser in expectedUsers) expected[expectedUser.UserName] = expectedUser.UserLevel;
        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public async Task UpdateUser()
    {
        var user = new User { UserName = "TestUser1", UserLevel = UserLevel.Admin };
        var result = await _userService.UpdateUser(user);
        var updatedUser = await _dbContext.GoalUsers.FirstOrDefaultAsync(u => u.UserName == user.UserName);
        Assert.That(result);
        Assert.That(updatedUser.UserLevel, Is.EqualTo(user.UserLevel));
    }

    private string HashPassword(string pass)
    {
        byte[] salt = { 187, 69, 193, 241, 190, 187, 23, 10, 114, 164, 239, 80, 79, 38, 7, 93 };

        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            pass!,
            salt,
            KeyDerivationPrf.HMACSHA256,
            100000,
            256 / 8));

        return hashed;
    }
}
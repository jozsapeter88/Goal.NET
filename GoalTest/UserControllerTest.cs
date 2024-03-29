﻿using System.Net;
using System.Security.Claims;
using Backend.Controllers;
using Backend.Enums;
using Backend.Model;
using Backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GoalTest;

[TestFixture]
public class UserControllerTest
{
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<GoalContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _dbContext = new GoalContext(options);
        _userService = new UserService(_dbContext);
        _userController = new UserController(_userService);

        _dbContext.GoalUsers.AddRange(
            new User { UserName = "TestUser1", Password = HashPassword("1234"), UserLevel = UserLevel.User },
            new User { UserName = "TestUser2", Password = HashPassword("1234"), UserLevel = UserLevel.Operator },
            new User { UserName = "TestUser3", Password = HashPassword("1234"), UserLevel = UserLevel.Admin }
        );
        _dbContext.SaveChanges();
        var authServiceMock = new Mock<IAuthenticationService>();
        authServiceMock
            .Setup(_ => _.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(),
                It.IsAny<AuthenticationProperties>()))
            .Returns(Task.FromResult((object)null));

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock
            .Setup(_ => _.GetService(typeof(IAuthenticationService)))
            .Returns(authServiceMock.Object);

        var httpContext = new DefaultHttpContext
        {
            RequestServices = serviceProviderMock.Object
        };
        _userController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
    }


    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    private UserController _userController;
    private IUserService _userService;
    private GoalContext _dbContext;

    [Test]
    public async Task LoginTest()
    {
        // Act
        var result = await _userController.UserLogin(new User { UserName = "TestUser2", Password = "1234" });
        var statusCodeResult = result as OkObjectResult;
        var expected = (int)HttpStatusCode.OK;
        // Assert
        Assert.IsNotNull(result);
        Assert.That(statusCodeResult.StatusCode, Is.EqualTo(expected));
    }

    [Test]
    public async Task LoginExceptionTest()
    {
        // Act
        var result = await _userController.UserLogin(new User { UserName = "TestUser2", Password = "testpassword" });
        var statusCodeResult = result as UnauthorizedResult;
        var expected = (int)HttpStatusCode.Unauthorized;
        // Assert
        Assert.IsNotNull(result);
        Assert.That(statusCodeResult.StatusCode, Is.EqualTo(expected));
    }

    [Test]
    public async Task UpdateTest()
    {
        // Act
        var result = _userController.UpdateUser(new User { UserName = "TestUser2", UserLevel = UserLevel.Admin });
        var statusCodeResult = result as OkResult;
        var expected = (int)HttpStatusCode.OK;
        // Assert
        Assert.IsNotNull(result);
        Assert.That(statusCodeResult.StatusCode, Is.EqualTo(expected));
    }

    [Test]
    public async Task RegisterTest()
    {
        // Act
        var result = await _userController.RegisterUser(new User { UserName = "TestUser4", Password = "1234" });
        var statusCodeResult = result as OkResult;
        var expected = (int)HttpStatusCode.OK;
        // Assert
        Assert.IsNotNull(result);
        Assert.That(statusCodeResult.StatusCode, Is.EqualTo(expected));
    }

    [Test]
    public async Task GetAllUsersTest()
    {
        // Act
        var result = await _userController.ProvideUsers();

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public async Task GetLevelsTest()
    {
        // Act
        var result = await _userController.ProvideUserLevels();

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
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
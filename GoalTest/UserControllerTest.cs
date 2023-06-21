using System.Net;
using System.Security.Claims;
using AutoMapper;
using Backend.Controllers;
using Backend.DTOs;
using Backend.Enums;
using Backend.Model;
using Backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using Moq;
using NuGet.Protocol;

namespace GoalTest;

[TestFixture]
public class UserControllerTest
{
        private UserController _userController; 
        private IUserService _userService;
        private GoalContext _dbContext;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<GoalContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new GoalContext(options);
            _userService = new UserService(_dbContext);
            _userController = new UserController(_userService);
            
            _dbContext.GoalUsers.AddRange(
                new User { UserName = "TestUser1", Password = HashPassword("1234") , UserLevel = UserLevel.User },
                new User { UserName = "TestUser2", Password = HashPassword("1234"), UserLevel = UserLevel.Operator },
                new User { UserName = "TestUser3", Password = HashPassword("1234"), UserLevel = UserLevel.Admin }
            );
            _dbContext.SaveChanges();
            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(_ => _.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.FromResult((object)null));

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(_ => _.GetService(typeof(IAuthenticationService)))
                .Returns(authServiceMock.Object);
            
            var httpContext = new DefaultHttpContext()
            {
                RequestServices = serviceProviderMock.Object
            };
            _userController.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            

            
        }

        [Test]
        public async Task Login()
        {
            // Act
            ActionResult result = await _userController.UserLogin(new User { UserName = "TestUser2", Password = "1234" });
            var statusCodeResult = result as OkObjectResult;
            var expected = (int)HttpStatusCode.OK;
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, statusCodeResult.StatusCode);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
        
        private string HashPassword(string pass)
        {
            byte[] salt = { 187, 69, 193, 241, 190, 187, 23, 10, 114, 164, 239, 80, 79, 38, 7, 93 };

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: pass!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
}
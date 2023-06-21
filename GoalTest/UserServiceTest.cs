using System.Net;
using AutoMapper;
using Backend.Controllers;
using Backend.DTOs;
using Backend.Enums;
using Backend.Model;
using Backend.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GoalTest;

[TestFixture]
public class UserServiceTest
{
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

            _dbContext.GoalUsers.AddRange(
                new User { UserName = "TestUser1", Password = HashPassword("1234") , UserLevel = UserLevel.User },
                new User { UserName = "TestUser2", Password = HashPassword("1234"), UserLevel = UserLevel.Operator },
                new User { UserName = "TestUser3", Password = HashPassword("1234"), UserLevel = UserLevel.Admin }
            );
            _dbContext.SaveChanges();

            
        }

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
            string username = "TestUser1";
            var result = await _userService.GetUser(username);
            var expected = await _dbContext.GoalUsers.FirstOrDefaultAsync(u => u.UserName == username);
            Assert.That(expected, Is.EqualTo(result));
        }
        
        [Test]
        public async Task GetAllUsers()
        {
            var result = await _userService.GetAllUsers();
            Dictionary<string, UserLevel> expected = new Dictionary<string, UserLevel>();
            var expectedUsers = await _dbContext.GoalUsers.ToListAsync();
            foreach (var expectedUser in expectedUsers)
            {
                expected[expectedUser.UserName] = expectedUser.UserLevel;
            }
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
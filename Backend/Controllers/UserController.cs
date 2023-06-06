using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Backend.Enums;
using Backend.Model;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Extensions;
using NuGet.Protocol;

namespace Backend.Controllers;
[ApiController]
[Route("/api/user")]
public class UserController : ControllerBase
{
   private IUserService UserService { get; }
   public UserController(IUserService userService)
   {
      UserService = userService;
   }
   [HttpPost("login")]
   public ActionResult UserLogin([FromBody] User user)
   {
      if (UserService.Login(user.UserName, user.Password).Result)
      {
         User userFromDb = UserService.GetUser(user.UserName).Result;
         var token = GenerateJWT(userFromDb);
         return Ok(token);
      }
      return Unauthorized();
   }

   [HttpPost("register")]
   
   public ActionResult RegisterUser([FromBody] User user)
   { 
      if (UserService.Register(user.UserName, user.Password).Result)
      {
         var token = GenerateJWT(user);
         return Ok(token);
      }
      return Unauthorized();
   }

   [HttpGet("levels")]
   [Authorize(Roles = "Admin")]
   public ActionResult<List<string>> ProvideUserLevels()
   {
      
      var userLevels = Enum.GetNames(typeof(UserLevel)).ToList();
      return Ok(userLevels);
   }
   
   [HttpGet("getAll")]
   [Authorize(Roles = "Operator,Admin")]
   public ActionResult<Dictionary<string, UserLevel>> ProvideUsers()
   {
      var users = UserService.GetAllUsers();
      return Ok(users.Result);
   }

   [HttpPost("update")]
   [Authorize(Roles = "Admin")]
   public ActionResult UpdateUser([FromBody] User user)
   {
      if (UserService.UpdateUser(user).Result)
      {
         return Ok();
      }

      return Unauthorized();
   }

   private string Generate(User user)
   {
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("GoalDotNetIsTheBestProjectOfCodeCool"));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
      var expirationTime = DateTime.UtcNow.AddHours(1);
      var token = new JwtSecurityToken(
         claims: new[]
         {
            new Claim(ClaimTypes.Authentication, $"{user.UserName}:{user.Password}")
         },
         expires: expirationTime,
         signingCredentials: creds
         );
 
      return new JwtSecurityTokenHandler().WriteToken(token);
   }
   private string GenerateJWT(User user)
   {
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("GoalDotNetIsTheBestProjectOfCodeCool"));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

      var claims = new[]
      {
         new Claim(ClaimTypes.NameIdentifier, user.UserName),
         new Claim(ClaimTypes.Role, user.UserLevel.ToString())
      };

      var token = new JwtSecurityToken("http://localhost:5076/",
         "http://localhost:5076/",
         claims,
         expires: DateTime.Now.AddHours(1),
         signingCredentials: credentials);

      return new JwtSecurityTokenHandler().WriteToken(token);
   }
}
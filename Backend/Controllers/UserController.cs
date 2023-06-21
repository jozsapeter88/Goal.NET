﻿using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Backend.Enums;
using Backend.Model;
using Backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
   public async Task<ActionResult> UserLogin([FromBody] User user)
   {
      if (await UserService.Login(user.UserName, user.Password))
      {
         var userFromDb = UserService.GetUser(user.UserName).Result;
         var token = await GenerateJwt(userFromDb);
         return Ok(token);
      }
      return Unauthorized();
   }

   [HttpPost("register")]
   
   public async Task<ActionResult> RegisterUser([FromBody] User user)
   { 
      if (UserService.Register(user.UserName, user.Password).Result)
      {
         var token = await GenerateJwt(user);
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

  /* private string Generate(User user)
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
   }*/
   private async Task<string> GenerateJwt(User user)
   {
      //get parameters from config
      var configuration = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json")
         .Build();
      var key = configuration.GetSection("JWT")["key"];
      var issuer = configuration.GetSection("JWT")["Issuer"];
      var audience = configuration.GetSection("JWT")["Audience"];
      
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

      var claims = new[]
      {
         new Claim(ClaimTypes.NameIdentifier, user.UserName),
         new Claim(ClaimTypes.Role, user.UserLevel.ToString())
      };
      
      var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
      var claimsPrincipal = new ClaimsPrincipal(identity);
      await HttpContext.SignInAsync("Cookies",claimsPrincipal);

      var token = new JwtSecurityToken(
         issuer,
         audience,
         claims,
         expires: DateTime.Now.AddHours(1),
         signingCredentials: credentials);

      return new JwtSecurityTokenHandler().WriteToken(token);
   }
}
using System.Runtime.InteropServices.JavaScript;
using Backend.Enums;
using Backend.Model;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
         return Ok();
      }
      return Unauthorized();
   }

   [HttpPost("register")]
   public ActionResult RegisterUser([FromBody] User user)
   {
      if (UserService.Register(user.UserName, user.Password).Result)
      {
         return Ok();
      }
      return Unauthorized();
   }

   [HttpGet("levels")]
   public ActionResult<List<string>> ProvideUserLevels()
   {
      var userLevels = Enum.GetNames(typeof(UserLevel)).ToList();
      return Ok(userLevels);
   }
   
   [HttpGet("getAll")]
   public ActionResult<Dictionary<string, UserLevel>> ProvideUsers()
   {
      var users = UserService.GetAllUsers();
      return Ok(users.Result);
   }

   [HttpPost("update")]
   public ActionResult UpdateUser([FromBody] User user)
   {
      if (UserService.UpdateUser(user).Result)
      {
         return Ok();
      }

      return Unauthorized();
   }
}
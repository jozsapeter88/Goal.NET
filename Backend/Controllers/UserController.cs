using System.Runtime.InteropServices.JavaScript;
using Backend.Model;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;
[ApiController]
[Route("/user")]
public class UserController : ControllerBase
{
   private IUserService _userService { get; }
   public UserController(IUserService userService)
   {
      _userService = userService;
   }
   [HttpGet("login")]
   public ActionResult UserLogin([FromBody] User user)
   {
      if (_userService.Login(user.UserName, user.Password))
      {
         return Ok();
      }
      return NotFound();
   }
}
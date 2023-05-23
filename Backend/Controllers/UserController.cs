using System.Runtime.InteropServices.JavaScript;
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
   public ActionResult UserLogin([FromBody] string username, [FromBody] string password)
   {
      if (_userService.Login(username, password))
      {
         return Ok();
      }
      return NotFound();
   }
}
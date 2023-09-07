using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Enums;
using Backend.Model;
using Backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Controllers;

[ApiController]
[Route("/api/user")]
public class UserController : ControllerBase
{
    public UserController(IUserService userService)
    {
        UserService = userService;
    }

    private IUserService UserService { get; }

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
            return Ok();
        }

        return Unauthorized();
    }

    [HttpGet("levels")]
    public async Task<ActionResult<List<string>>> ProvideUserLevels()
    {
        var userLevels = Enum.GetNames(typeof(UserLevel)).ToList();
        return Ok(userLevels);
    }

    [HttpGet("getAll")]
    [Authorize(Roles = "Operator,Admin")]
    public async Task<ActionResult<Dictionary<string, UserLevel>>> ProvideUsers()
    {
        var users = await UserService.GetAllUsers();
        return Ok(users);
    }

    [HttpPost("update")]
    [Authorize(Roles = "Admin")]
    public ActionResult UpdateUser([FromBody] User user)
    {
        if (UserService.UpdateUser(user).Result) return Ok();

        return Unauthorized();
    }

    [HttpGet("level")]
    public async Task<ActionResult> GetUserLevel()
    {
        var user = await GetCurrentUser();
        if (user == null)
        {
            return NotFound("User might not be logged in");
        }
        return Ok(user.UserLevel.ToString());
    }

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
        await HttpContext.SignInAsync("Cookies", claimsPrincipal);

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: DateTime.Now.AddHours(10),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    [HttpGet("currentUser")]
    private async Task<User?> GetCurrentUser()
    {
        {
            if (HttpContext.User.Identity is not ClaimsIdentity identity) return null;
            var userClaims = identity.Claims;

            var username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
            if (username != null)
                return await UserService.GetUser(username);
            return null;
        }
    }
}
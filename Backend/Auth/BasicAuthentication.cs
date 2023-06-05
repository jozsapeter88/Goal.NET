using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Backend.Enums;
using Backend.Model;
using Backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;

namespace Backend.Auth;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private IUserService UserService { get; } 
    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        GoalContext dbContext) : base(options, logger, encoder, clock)
    {
        UserService = new UserService(dbContext);
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var endpoint = Context.GetEndpoint();

        if (endpoint?.Metadata?.GetMetadata<Authorization>() == null)
        {
            return AuthenticateResult.NoResult();
        }
        string token = "";
        //  Get login parameters from cookies
        if (Request.Cookies.ContainsKey("token"))
        {
          token = Request.Cookies["token"].Split(" ")[1];
        }
        string[] tokenParts = Encoding.UTF8.GetString(Convert.FromBase64String(token)).Split(":");
        string username = tokenParts[0];
        string password = tokenParts[1];
        if (AuthenticateUser(username, password))
        {
            var user = UserService.GetUser(username).Result;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.UserLevel.ToString())
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, CookieAuthenticationDefaults.AuthenticationScheme);
            return AuthenticateResult.Success(ticket);
        }

        return AuthenticateResult.Fail("Invalid login credentials");

    }

    private bool AuthenticateUser(string username, string password)
    {
        User user;
        try
        {
            user = UserService.GetUser(username).Result;
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
            
        return user.Password.Equals(password);
    }
}
using System.Net;
using System.Security.Claims;
using AutoMapper;
using Backend.Controllers;
using Backend.DTOs.AutoMapper;
using Backend.Enums;
using Backend.Model;
using Backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NuGet.Protocol;

namespace GoalTest;

public class TeamControllerTests
{
    private TeamController _teamController;
    private UserController _userController;
    private  Mock<TeamService> _mockTeamService;
    private  Mock<UserService> _mockUserService;
    private GoalContext _dbContext;
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new AutoMapperProfile());
        });
        IMapper mapper = mappingConfig.CreateMapper();
        _mapper = mapper;

        
        var options = new DbContextOptionsBuilder<GoalContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _dbContext = new GoalContext(options);

        _dbContext.Players.AddRange(
            new Player { Name = "TestPlayer1", Position = PositionEnum.Forward },
            new Player { Name = "TestPlayer2", Position = PositionEnum.Defender }
        );
        _dbContext.Teams.AddRange(
            new Team { Name = "Team 1", Overall = 90, Color = "Red" },
            new Team { Name = "Team 2", Overall = 80, Color = "Yellow" },
            new Team { Name = "Team 3", Overall = 60, Color = "Blue" });

        _dbContext.GoalUsers.AddRange(
            new User
            {
                UserName = "Tester1",
                Password = HashPassword("1234"),
                Teams = new List<Team>
                {
                    new Team
                    {
                        Name = "Team 4",
                        Overall = 76,
                        Color = "Pink",
                        AllPlayers = new List<Player>
                        {
                            new Player { Name = "TestPlayer3", Position = PositionEnum.Forward },
                            new Player { Name = "TestPlayer4", Position = PositionEnum.Defender }
                        }
                    },
                    new Team
                    {
                        Name = "Team 5",
                        Overall = 80,
                        Color = "Orange",
                        AllPlayers = new List<Player>
                        {
                            new Player { Name = "TestPlayer5", Position = PositionEnum.Forward },
                            new Player { Name = "TestPlayer6", Position = PositionEnum.Defender }
                        }
                    }
                },
                UserLevel = UserLevel.User
            });
        _dbContext.SaveChangesAsync();
        
        _mockTeamService = new Mock<TeamService>(MockBehavior.Default, _dbContext, _mapper);
        _mockUserService = new Mock<UserService>(MockBehavior.Default, _dbContext);
        _userController = new UserController(_mockUserService.Object);
        _teamController = new TeamController(_mockTeamService.Object, _mockUserService.Object);
        
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
    public async Task GetAllTeams_ReturnsStatusCodeOk_Test()
    {
        var teams = await _teamController.GetAllTeams();
        var result = teams.Result as OkObjectResult;
        var expected = (int)HttpStatusCode.OK;
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        Assert.That(expected, Is.EqualTo(result?.StatusCode));
    }

    [Test]
    public async Task GetTeam_ReturnsStatusCodeOk_Test()
    {
        var team = await _teamController.GetTeam(2);
        var result = team.Result as OkObjectResult;
        var expected = (int)HttpStatusCode.OK;
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        Assert.That(expected, Is.EqualTo(result?.StatusCode));
    }
    
    [Test]
    public async Task GetTeam_ReturnsStatusCodeNotFound_Test()
    {
        var team = await _teamController.GetTeam(7);
        var result = team.Result as NotFoundObjectResult;
        var expected = (int)HttpStatusCode.NotFound;
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        Assert.That(expected, Is.EqualTo(result?.StatusCode));
    }

    [Test]
    public async Task GetTeamOfLoggedInUserReturnsOk_Test()
    {
        await _userController.UserLogin(new User { UserName = "Tester1", Password = "1234" });
        var teams = await _teamController.GetTeamsOfUser();
        var result = teams.Result as OkObjectResult;
        var expected = (int)HttpStatusCode.OK;
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        Assert.That(expected, Is.EqualTo(result?.StatusCode));
        
    }
    
    private string HashPassword(string pass)
    {
        byte[] salt = { 187, 69, 193, 241, 190, 187, 23, 10, 114, 164, 239, 80, 79, 38, 7, 93 };
        Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");
        
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: pass!,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

        return hashed;
    }
}
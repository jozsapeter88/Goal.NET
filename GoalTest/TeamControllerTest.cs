using System.Net;
using System.Security.Claims;
using AutoMapper;
using Backend.Controllers;
using Backend.DTOs;
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

namespace GoalTest;

[TestFixture]
public class TeamControllerTests
{
    [SetUp]
    public void Setup()
    {
        //AutoMapper config
        var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new AutoMapperProfile()); });
        var mapper = mappingConfig.CreateMapper();
        _mapper = mapper;
        //InMemoryDatabase config
        var options = new DbContextOptionsBuilder<GoalContext>()
            .UseInMemoryDatabase("TestDatabase")
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
                    new()
                    {
                        Name = "Team 4",
                        Overall = 76,
                        Color = "Pink",
                        AllPlayers = new List<Player>
                        {
                            new() { Name = "TestPlayer3", Position = PositionEnum.Forward },
                            new() { Name = "TestPlayer4", Position = PositionEnum.Defender }
                        }
                    },
                    new()
                    {
                        Name = "Team 5",
                        Overall = 80,
                        Color = "Orange",
                        AllPlayers = new List<Player>
                        {
                            new() { Name = "TestPlayer5", Position = PositionEnum.Forward },
                            new() { Name = "TestPlayer6", Position = PositionEnum.Defender }
                        }
                    }
                },
                UserLevel = UserLevel.User
            });
        _dbContext.SaveChangesAsync();
        //MockServices
        _mockTeamService = new Mock<ITeamService>();
        _mockUserService = new Mock<UserService>(MockBehavior.Default, _dbContext);
        //Controllers
        _userController = new UserController(_mockUserService.Object);
        _teamController = new TeamController(_mockTeamService.Object, _mockUserService.Object);
        //Mock AuthenticationService 
        var authServiceMock = new Mock<IAuthenticationService>();
        authServiceMock
            .Setup(_ => _.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(),
                It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
            .Returns(Task.FromResult((object)null));
        //Mock ServiceProvider
        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock
            .Setup(_ => _.GetService(typeof(IAuthenticationService)))
            .Returns(authServiceMock.Object);
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(ClaimTypes.NameIdentifier, "Tester1")
        }, "mock"));
        //HttpContext
        var httpContext = new DefaultHttpContext
        {
            RequestServices = serviceProviderMock.Object,
            User = user
        };
        _teamController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    private TeamController _teamController;
    private UserController _userController;
    private Mock<ITeamService> _mockTeamService;
    private Mock<UserService> _mockUserService;
    private GoalContext _dbContext;
    private IMapper _mapper;

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
        var teams = await _teamController.GetTeamsOfUser();
        var result = teams.Result as OkObjectResult;
        var expected = (int)HttpStatusCode.OK;
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        Assert.That(expected, Is.EqualTo(result?.StatusCode));
    }

    [Test]
    public async Task CreateTeamReturnsOk_Test()
    {
        var teams = await _teamController.CreateTeam(new TeamCreateDto { Name = "TestTeam", Color = "Red" });
        var result = teams.Result as OkObjectResult;
        var expected = (int)HttpStatusCode.OK;
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        Assert.That(expected, Is.EqualTo(result?.StatusCode));
    }

    [Test]
    public async Task UpdateTeamReturnsOk_Test()
    {
        var team = await _teamController.UpdateTeam(1, new Team { Name = "TestTeam", Color = "Red" });
        var result = team.Result as OkObjectResult;
        var expected = (int)HttpStatusCode.OK;
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        Assert.That(expected, Is.EqualTo(result?.StatusCode));
    }

    [Test]
    public async Task GetPlayersOfTeamOfLoggedInUser_Test()
    {
        var players = await _teamController.GetPlayersOfTeam(4);
        var result = players?.Result as OkObjectResult;
        var expected = (int)HttpStatusCode.OK;
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        Assert.That(expected, Is.EqualTo(result?.StatusCode));
    }

    [Test]
    public async Task GetPlayersOfTeamOfLoggedInUserReturnsNotFound_Test()
    {
        var players = await _teamController.GetPlayersOfTeam(7);
        var result = players?.Result as NotFoundObjectResult;
        var expected = (int)HttpStatusCode.NotFound;
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        Assert.That(expected, Is.EqualTo(result?.StatusCode));
    }

    [Test]
    public async Task UpdateNameOfTeamOfLoggedInUserReturnsOk_Test()
    {
        var team = await _teamController.UpdateTeamName(4, "UpdateTeamNameTest");
        var result = team.Result as OkObjectResult;
        var expected = (int)HttpStatusCode.OK;
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        Assert.That(expected, Is.EqualTo(result?.StatusCode));
    }

    [Test]
    public async Task UpdateNameOfTeamOfLoggedInUserReturnsNotFound_Test()
    {
        var team = await _teamController.UpdateTeamName(1, "UpdateTeamNameTest");
        var result = team.Result as NotFoundObjectResult;
        var expected = (int)HttpStatusCode.NotFound;
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        Assert.That(expected, Is.EqualTo(result?.StatusCode));
    }

    [Test]
    public async Task DeleteTeamByUserReturnsOk_Test()
    {
        var action = await _teamController.UserDeleteTeam(4);
        var result = action.Result as OkObjectResult;
        var expected = (int)HttpStatusCode.OK;
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        Assert.That(expected, Is.EqualTo(result?.StatusCode));
    }

    [Test]
    public async Task DeleteTeamByUserReturnsNotFound_Test()
    {
        var action = await _teamController.UserDeleteTeam(7);
        var result = action.Result as NotFoundObjectResult;
        var expected = (int)HttpStatusCode.NotFound;
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        Assert.That(expected, Is.EqualTo(result?.StatusCode));
    }

    [Test]
    public async Task UpdateTeamReturnsNotFound_Test()
    {
        var exampleTeam = new Team { Name = "TestTeam", Color = "Red" };
        _mockTeamService.Setup(mock => mock.UpdateTeam(12345, exampleTeam)).Returns(Task.FromResult((Team)null));
        var team = await _teamController.UpdateTeam(12345, exampleTeam);
        var result = team.Result as NotFoundObjectResult;
        var expected = (int)HttpStatusCode.NotFound;
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        Assert.That(expected, Is.EqualTo(result?.StatusCode));
    }

    [Test]
    public async Task AddPlayerReturnsOk_Test()
    {
        var team = await _teamController.AddPlayer(4, 1);
        var result = team.Result as OkObjectResult;
        var expected = (int)HttpStatusCode.OK;
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        Assert.That(expected, Is.EqualTo(result?.StatusCode));
    }

    [Test]
    public async Task AddPlayerReturnsNotFound_Test()
    {
        var team = await _teamController.AddPlayer(1, 1);
        var result = team.Result as NotFoundObjectResult;
        var expected = (int)HttpStatusCode.NotFound;
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        Assert.That(expected, Is.EqualTo(result?.StatusCode));
    }

    [Test]
    public async Task DeleteTeamReturnsOk_Test()
    {
        var teams = await _teamController.DeleteTeam(1);
        var result = teams.Result as OkObjectResult;
        var expected = (int)HttpStatusCode.OK;
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        Assert.That(expected, Is.EqualTo(result?.StatusCode));
    }

    private string HashPassword(string pass)
    {
        byte[] salt = { 187, 69, 193, 241, 190, 187, 23, 10, 114, 164, 239, 80, 79, 38, 7, 93 };
        Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            pass!,
            salt,
            KeyDerivationPrf.HMACSHA256,
            100000,
            256 / 8));

        return hashed;
    }
}
using AutoMapper;
using Backend.Controllers;
using Backend.DTOs;
using Backend.DTOs.AutoMapper;
using Backend.Enums;
using Backend.Model;
using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoalTest;

[TestFixture]
public class PlayerControllerTest
{
    [SetUp]
    public void Setup()
    {
        var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new AutoMapperProfile()); });
        var mapper = mappingConfig.CreateMapper();
        _mapper = mapper;

        var options = new DbContextOptionsBuilder<GoalContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _dbContext = new GoalContext(options);

        _dbContext.Players.AddRange(
            new Player { Name = "TestForward", Position = PositionEnum.Forward },
            new Player { Name = "TestDefender", Position = PositionEnum.Defender },
            new Player { Name = "TestGoalkeeper", Position = PositionEnum.Goalkeeper },
            new Player { Name = "TestMidfielder", Position = PositionEnum.Midfielder }
        );
        _dbContext.SaveChanges();

        _playerService = new PlayerService(_dbContext, _mapper);
        _playerController = new PlayerController(_playerService);
    }


    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    private PlayerController _playerController;
    private IPlayerService _playerService;
    private GoalContext _dbContext;
    private IMapper _mapper;

    [Test]
    public async Task GetAllPlayer_ReturnsStatusCode200()
    {
        // Act
        var result = await _playerController.GetAllPlayers();

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public async Task GetGoalkeepers_ReturnsStatusCode200()
    {
        // Act
        var result = await _playerController.GetGoalkeepers();

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public async Task GetDefenders_ReturnsStatusCode200()
    {
        // Act
        var result = await _playerController.GetDefenders();

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public async Task GetMidfielders_ReturnsStatusCode200()
    {
        // Act
        var result = await _playerController.GetMidfielders();

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public async Task GetForwards_ReturnsStatusCode200()
    {
        // Act
        var result = await _playerController.GetForwards();

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public void GetNationalities_ReturnsStatusCode200()
    {
        // Act
        var result = _playerController.GetNationalities();

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public void GetPositions_ReturnsStatusCode200()
    {
        // Act
        var result = _playerController.GetPositions();

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public void GetGender_ReturnsStatusCode200()
    {
        // Act
        var result = _playerController.GetGender();

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public async Task CreatePlayer_ReturnsStatusCode200()
    {
        // Arrange
        var playerDto = new PlayerDto
        {
            Name = "CreatedPlayerTest",
            Position = PositionEnum.Defender,
            Nationality = NationalityEnum.Albania,
            Gender = GenderEnum.Male,
            Score = 67
        };

        // Act
        var result = await _playerController.CreatePlayer(playerDto);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public async Task DeletePlayer_ReturnsStatusCode200()
    {
        // Arrange
        long playerId = 1;

        // Act
        var result = await _playerController.DeletePlayer(playerId);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }
}
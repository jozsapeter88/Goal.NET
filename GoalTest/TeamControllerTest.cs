using System.Net;
using AutoFixture;
using Backend.Controllers;
using Backend.DTOs;
using Backend.Model;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Moq;

namespace GoalTest;

[TestFixture]
public class TeamControllerTest2
{
    private TeamController _teamController;
    private Mock<ITeamService> _mockTeamService;
    private Mock<IUserService> _mockUserService;
    private Fixture _fixture;

    public TeamControllerTest2()
    {
        
        _mockTeamService = new Mock<ITeamService>();
        _mockUserService = new Mock<IUserService>();
        _teamController = new TeamController(_mockTeamService.Object, _mockUserService.Object);
    }
    
    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Test]
    public async Task GetAllTeams_ReturnsStatusCodeOk_Test()
    {
        var teamList = _fixture.CreateMany<Team>(3).ToList();
        _mockTeamService.Setup(repo => repo.GetAllTeams()).Returns(Task.FromResult(teamList)!);
        var teams = await _teamController.GetAllTeams();
        var obj = teams.Result as ObjectResult;
        var expected = (int)HttpStatusCode.OK;
        Assert.That(expected, Is.EqualTo(obj.StatusCode));
    }
    
    [Test]
    public async Task GetAllTeams_ReturnsStatusCodeNotFound_Test()
    {
        _mockTeamService.Setup(repo => repo.GetAllTeams()).Returns(Task.FromResult<List<Team>>(result: null));
        var teams = await _teamController.GetAllTeams();
        var obj = teams.Result as ObjectResult;
        var expected = (int)HttpStatusCode.NotFound;
        Assert.That(expected, Is.EqualTo(obj.StatusCode));
    }
    
    [Test]
    public async Task GetTeam_ReturnsStatusCodeOk_Test()
    {
        var team = _fixture.Create<Team>();
        _mockTeamService.Setup(repo => repo.GetTeam(It.IsAny<long>())).Returns(Task.FromResult(team)!);
        var result = await _teamController.GetTeam(It.IsAny<long>());
        var obj = result.Result as ObjectResult;
        var expected = (int)HttpStatusCode.OK;
        Assert.That(expected, Is.EqualTo(obj.StatusCode));
    }
    
    [Test]
    public async Task CreateTeamReturnsOk_Test()
    {
        var exampleTeam = new TeamCreateDto { Name = "TestTeam", Color = "Red" };
        _mockTeamService.Setup(repo => repo.CreateTeam(exampleTeam)).Returns(Task.FromResult(result: new List<Team>()));
        var result = await _teamController.CreateTeam(exampleTeam);
        var obj = result.Result as ObjectResult;
        var expected = (int)HttpStatusCode.OK;
        Assert.That(expected, Is.EqualTo(obj.StatusCode));
    }
    
    [Test]
    public async Task UpdateTeamReturnsOk_Test()
    {
        var exampleTeam = new Team { Name = "TestTeam", Color = "Red" };
        _mockTeamService
            .Setup(mock => mock
                .UpdateTeam(It.IsAny<long>(), exampleTeam))
            .Returns(Task.FromResult(exampleTeam)!);
        var result = await _teamController.UpdateTeam(It.IsAny<long>(), exampleTeam);
        var obj = result.Result as ObjectResult;
        var expected = (int)HttpStatusCode.OK;
        Assert.That(expected, Is.EqualTo(obj?.StatusCode));
    }

    [Test]
    public async Task DeleteTeamReturnsOk_Test()
    {
        _mockTeamService.Setup(mock => mock.DeleteTeam(It.IsAny<long>())).Returns(Task.FromResult(true));
        var result = await _teamController.DeleteTeam(It.IsAny<long>());
        var obj = result as ObjectResult;
        var expected = (int)HttpStatusCode.OK;
        Assert.That(expected, Is.EqualTo(obj?.StatusCode));
    }

   
}
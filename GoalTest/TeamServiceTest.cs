using AutoMapper;
using Backend.DTOs;
using Backend.DTOs.AutoMapper;
using Backend.Enums;
using Backend.Model;
using Backend.Services;
using Microsoft.EntityFrameworkCore;

namespace GoalTest;

public class TeamServiceTest
{
    private static IMapper _mapper;
    private GoalContext _dbContext;
    private ITeamService _teamService;


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
                Password = "1234",
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

        _dbContext.SaveChanges();

        _teamService = new TeamService(_dbContext, _mapper);
    }

    [Test]
    public async Task GetAllTeams_Test()
    {
        var result = await _teamService.GetAllTeams();
        var actual = _dbContext.Teams.Count();
        Console.WriteLine(result);
        Assert.That(result?.Count, Is.EqualTo(actual));
    }

    [Test]
    public async Task GetOneTeam_Test()
    {
        var result = await _teamService.GetTeam(1);
        var actual = _dbContext.Teams.FirstOrDefaultAsync(t => t.Id == 1).Result;

        Assert.NotNull(result);
        Assert.NotNull(actual);
        Assert.That(result?.Name, Is.EqualTo(actual?.Name));
        Assert.That(result?.Overall, Is.EqualTo(actual?.Overall));
        Assert.That(result?.Color, Is.EqualTo(actual?.Color));
    }

    [Test]
    public async Task AddTeamToUser_Test()
    {
        var team = new TeamCreateDto { Name = "Team6", Color = "Yellow" };
        var result = await _teamService.AddTeamToUser(1, team);
        var user = await _dbContext.GoalUsers.FirstOrDefaultAsync(u => u.Id == 1);
        var userTeams = user?.Teams?.Count;
        Console.WriteLine(userTeams);
        Assert.That(result?.Count, Is.EqualTo(userTeams));
    }

    [Test]
    public async Task CreateTeamByAdmin_Test()
    {
        var team = new TeamCreateDto { Name = "TeamByAdmin", Color = "Red" };
        var result = await _teamService.CreateTeam(team);
        Console.WriteLine(result.Count());
        var allTeams = _dbContext.Teams.Count();
        Console.WriteLine(allTeams);
        Assert.That(result.Count, Is.EqualTo(allTeams));
    }

    [Test]
    public async Task AddPlayerToUsersTeam_Test()
    {
        var team = await _teamService.AddPlayerToTeam(1, 4, 1);
        var result = team?.AllPlayers?.Count;
        var user = await _dbContext.GoalUsers.FirstOrDefaultAsync(u => u.Id == 1);
        var teamOfUser = user?.Teams?.FirstOrDefault(t => t.Id == 4);
        var numberOfPlayers = teamOfUser?.AllPlayers?.Count;
        Console.WriteLine(result);
        Console.WriteLine(numberOfPlayers);
        Assert.That(result, Is.EqualTo(numberOfPlayers));
    }

    [Test]
    public async Task UpdateTeamByAdmin_Test()
    {
        var updatedTeam = new Team
        {
            Name = "UpdatedTeam",
            Color = "Blue",
            Coach = new Coach { Name = "Coach Test", Gender = GenderEnum.Male, Nationality = NationalityEnum.Albania },
            AllPlayers = new List<Player>
            {
                new()
                {
                    Name = "UpdateTest Player",
                    Gender = GenderEnum.Male,
                    Nationality = NationalityEnum.Angola,
                    Position = PositionEnum.Goalkeeper,
                    Score = 76
                }
            }
        };
        var team = await _dbContext.Teams.FirstOrDefaultAsync(t => t.Id == 2);
        Console.WriteLine(team?.Name);
        var result = await _teamService.UpdateTeam(2, updatedTeam);
        Console.WriteLine(result?.Name);
        Console.WriteLine(result?.Coach?.Team?.Id);
        Console.WriteLine(updatedTeam.Coach?.Team?.Id);
        Console.WriteLine(result?.Coach?.Team?.Name);
        Console.WriteLine(updatedTeam.Coach?.Team?.Name);
        Assert.That(result?.Name, Is.EqualTo(updatedTeam.Name));
        Assert.That(result?.Coach, Is.EqualTo(updatedTeam.Coach));
        Assert.That(result?.Coach?.Team?.Id, Is.EqualTo(updatedTeam.Coach?.Team?.Id));
        Assert.That(result?.Coach?.Team?.Name, Is.EqualTo(updatedTeam.Coach?.Team?.Name));
    }

    [Test]
    public async Task UpdateTeamNameByUser_Test()
    {
        var team = await _dbContext.Teams.FirstOrDefaultAsync(t => t.Id == 4);
        var teamNameBefore = team?.Name;
        Console.WriteLine(teamNameBefore);
        var result = await _teamService.UpdateTeamName(1, 4, "NewTestTeamName");
        var teamNameAfter = team?.Name;
        Console.WriteLine(teamNameAfter);
        Assert.That(result?.Name, Is.EqualTo(teamNameAfter));
    }

    [Test]
    public async Task GetTeamsOfUser_Test()
    {
        var result = await _teamService.GetTeamsOfUser(1);
        var user = await _dbContext.GoalUsers.FirstOrDefaultAsync(u => u.Id == 1);
        var actual = 0;
        if (user is not null && user.Teams is not null) actual = user.Teams.Count;
        Console.WriteLine(actual);
        Console.WriteLine(result.Count);
        Assert.NotNull(result);
        Assert.NotNull(user);
        Assert.That(result?.Count, Is.EqualTo(actual));
    }

    [Test]
    public async Task GetPlayersOfUsersTeam_Test()
    {
        var result = await _teamService.GetPlayersOfTeam(1, 4);
        var user = await _dbContext.GoalUsers.FirstOrDefaultAsync(u => u.Id == 1);
        var numberOfPlayers = 0;
        var team = user?.Teams?.FirstOrDefault(t => t.Id == 4);
        Console.WriteLine(team.Name);
        if (team?.AllPlayers != null) numberOfPlayers = team.AllPlayers.Count;

        Console.WriteLine(result?.Count);
        Console.WriteLine(numberOfPlayers);
        Assert.NotNull(user);
        Assert.NotNull(user?.Teams);
        Assert.That(result?.Count, Is.EqualTo(numberOfPlayers));
    }

    [Test]
    public async Task NoUserFoundWhenTryToGetPlayersOfUsersTeam_ReturnNull()
    {
        var result = await _teamService.GetPlayersOfTeam(0, 4);
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task NoTeamsForUserFoundWhenTryToGetPlayersOfUsersTeam_ReturnNull()
    {
        var result = await _teamService.GetPlayersOfTeam(1, 6);
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task DeleteTeamByAdmin_Test()
    {
        var result = await _teamService.DeleteTeam(1);
        var allTeams = _dbContext.Teams.Count();
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task DeleteUsersTeamByUser_Test()
    {
        var user = await _dbContext.GoalUsers.FirstOrDefaultAsync(u => u.Id == 1);
        var usersTeamsBefore = user?.Teams?.Count;
        Console.WriteLine(usersTeamsBefore);
        var result = await _teamService.UserDeleteTeam(1, 5);
        var usersTeamsAfter = user?.Teams?.Contains(result!);
        Console.WriteLine(usersTeamsAfter);
        Assert.That(usersTeamsAfter, Is.False);
    }

    [Test]
    public async Task NoUserFoundWhenTryToDeleteTeam_ReturnNull()
    {
        var result = await _teamService.UserDeleteTeam(0, 5);
        Assert.That(result, Is.Null);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}
using Backend.Exception;
using Backend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class TeamService : ITeamService
{
    private readonly GoalContext _context;

    public TeamService(GoalContext context)
    {
        _context = context;
    }

    public async Task<List<Team>> GetAllTeams()
    {
        return await _context.Teams.ToListAsync();
    }

    public async Task<Team> GetTeam(long teamId)
    {
        var team = await _context.Teams.FindAsync(teamId);
         return team ?? throw new InvalidOperationException();
    }

    public async Task<Team> CreateTeam([FromBody] Team team)
    {
        var newTeam = new Team()
        {
            Name = team.Name,
            Color = team.Color
        };
        _context.Teams.Add(newTeam);
        await _context.SaveChangesAsync();
        return newTeam;
    }

    public async Task<Team> UpdateTeam(long teamId, Team team)
    {
        var teamToUpdate = await _context.Teams.FindAsync(teamId);
        if (teamToUpdate != null)
        {
            teamToUpdate.Coach = team.Coach;
            teamToUpdate.Name = team.Name;
            teamToUpdate.Color = team.Color;
            teamToUpdate.AllPlayers = team.AllPlayers;
        }
        await _context.SaveChangesAsync();
        return teamToUpdate ?? throw new NotFoundException("Team Not Found!");
    }

    public async Task<List<Team>> DeleteTeam(long teamId)
    {
        var teamToUpdate = await _context.Teams.FindAsync(teamId);
        if (teamToUpdate != null) _context.Teams.Remove(teamToUpdate);
        await _context.SaveChangesAsync();
        return await _context.Teams.ToListAsync();
    }

    public async Task<Team> AddPlayerToTeam(long teamId, long playerId)
    {
        var team = await _context.Teams.FindAsync(teamId);
        var player = await _context.Players.FindAsync(playerId);
        if (team != null)
        {
            team.AllPlayers = new List<Player> { player! };
        }

        await _context.SaveChangesAsync();
        return team ?? throw new NotFoundException("Team Not Found!");
    }
}
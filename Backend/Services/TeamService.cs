using AutoMapper;
using Backend.DTOs;
using Backend.Exception;
using Backend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class TeamService : ITeamService
{
    private readonly GoalContext _context;
    private readonly IMapper _mapper;

    public TeamService(GoalContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Team>> GetAllTeams()
    {
        return await _context.Teams.ToListAsync();
    }

    public async Task<Team> GetTeam(long teamId)
    {
        var team = await _context.Teams.Include(t => t.AllPlayers)
            .FirstOrDefaultAsync(t => t.Id == teamId);
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
        var team = await _context.Teams
            .Include(t => t.AllPlayers)
            .FirstOrDefaultAsync(t => t.Id == teamId);
        var player = await _context.Players
            .Include(p => p.Team)
            .FirstOrDefaultAsync(p => p.Id == playerId);
        if (team != null)
        {
            if (player != null)
            {
                if (player.Team != null)
                {
                    player.Team.Add(team);
                }
                else
                {
                    player.Team = new List<Team> { team };
                }

                if (team.AllPlayers != null)
                {
                    team.AllPlayers.Add(player);
                }
                else
                {
                    team.AllPlayers = new List<Player> { player };
                }
                
            }
        }
        await _context.SaveChangesAsync();
        return team ?? throw new NotFoundException("Team Not Found!");
    }

    public async Task<List<Team>> AddTeamToUser(long userId, TeamCreateDto team)
    {
        var user = await _context.GoalUsers.FindAsync(userId);
        var newTeam = _mapper.Map<Team>(team);
        if (user != null)
        {
            if (user.Teams == null)
            {
                user.Teams = new List<Team> { newTeam };
            }
            else
            {
                user.Teams.Add(newTeam);
            }
        }

        await _context.SaveChangesAsync();

        return user?.Teams ?? throw new NotFoundException("User Not Found!");
    }

    public Task<List<Team>> GetTeamsOfAUser(long userId, Team team)
    {
        throw new NotImplementedException();
    }
}
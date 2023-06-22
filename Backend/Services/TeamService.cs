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

    public async Task<List<Team>?> GetAllTeams()
    {
        var teams = await _context.Teams.ToListAsync();
        return teams ?? null;
    }

    public async Task<Team?> GetTeam(long teamId)
    {
        var team = await _context.Teams.Include(t => t.AllPlayers)
            .FirstOrDefaultAsync(t => t.Id == teamId);
        return team ?? null;
    }

    public async Task<List<Team>?> GetTeamsOfUser(long userId)
    {
        var user = await _context.GoalUsers
            .Include(u => u.Teams)
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user?.Teams?.ToList();
    }

    public async Task<List<Player>?> GetPlayersOfTeam(long userId, long teamId)
    {
        var user = await _context.GoalUsers
            .Include(u => u.Teams)
            .FirstOrDefaultAsync(u => u.Id == userId);
        var team = await _context.Teams
            .Include(t => t.AllPlayers)
            .FirstOrDefaultAsync(t => t.Id == teamId);
        if (user == null || team == null) return null;
        if (user.Teams == null || !user.Teams.Contains(team)) return null;
        return team.AllPlayers?.ToList();
    }


    public async Task<List<Team>> CreateTeam(TeamCreateDto team)
    {
        var newTeam = new Team()
        {
            Name = team.Name,
            Color = team.Color
        };
        _context.Teams.Add(newTeam);
        await _context.SaveChangesAsync();
        return await _context.Teams.ToListAsync();
    }

    public async Task<Team?> UpdateTeam(long teamId, Team team)
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
        return teamToUpdate ?? null;
    }

    public async Task<Team?> UpdateTeamName(long userId, long teamId, string teamName)
    {
        var user = await _context.GoalUsers
            .Include(u => u.Teams)
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user?.Teams == null) return null;
        var teamToUpdate = user.Teams.FirstOrDefault(t => t.Id == teamId);
        if (teamToUpdate != null)
        {
            teamToUpdate.Name = teamName;
        }

        await _context.SaveChangesAsync();
        return teamToUpdate;
    }

    public async Task<List<Team>> DeleteTeam(long teamId)
    {
        var teamToUpdate = await _context.Teams.FindAsync(teamId);
        if (teamToUpdate != null) _context.Teams.Remove(teamToUpdate);
        await _context.SaveChangesAsync();
        return await _context.Teams.ToListAsync();
    }

    public async Task<List<Team>?> UserDeleteTeam(long userId, long teamId)
    {
        var user = await _context.GoalUsers
            .Include(u => u.Teams)
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user != null && user.Teams != null)
        {
            var team = user.Teams.FirstOrDefault(t => t.Id == teamId);

            if (team != null)
            {
                user.Teams.Remove(team);
                if (team.AllPlayers != null)
                {
                    foreach (var player in team.AllPlayers)
                    {
                        player.Team.Remove(team);
                    }
                }

                _context.Teams.Remove(team);
            }
            await _context.SaveChangesAsync();
            return user.Teams.ToList();
        }

        return null;
    }

    public async Task<Team?> AddPlayerToTeam(long userId, long teamId, long playerId)
    {
        var user = await _context.GoalUsers
            .Include(u => u.Teams)
            .FirstOrDefaultAsync(u => u.Id == userId);
        var team = user?.Teams?
            .FirstOrDefault(t => t.Id == teamId);
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
        return team ?? null;
    }

    public async Task<List<Team>?> AddTeamToUser(long userId, TeamCreateDto team)
    {
        var user = await _context.GoalUsers.FindAsync(userId);
        var newTeam = _mapper.Map<Team>(team);
        _context.Teams.Add(newTeam);
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

        return user?.Teams ?? null;
    }
}
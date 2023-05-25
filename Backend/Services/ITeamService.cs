using Backend.DTOs;
using Backend.Model;

namespace Backend.Services;

public interface ITeamService
{
    Task<List<Team>> GetAllTeams();
    Task<Team> GetTeam(long teamId);
    Task<Team> CreateTeam(Team team);
    Task<Team> UpdateTeam(long teamId, Team team);
    Task<List<Team>> DeleteTeam(long teamId);

    Task<Team> AddPlayerToTeam(long teamId, long playerId);

    Task<List<Team>> AddTeamToUser(long userId, TeamCreateDto team);
    Task<List<Team>> GetTeamsOfAUser(long userId, Team team);
}
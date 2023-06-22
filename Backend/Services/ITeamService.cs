using Backend.DTOs;
using Backend.Model;

namespace Backend.Services;

public interface ITeamService
{
    Task<List<Team>?> GetAllTeams();
    Task<Team?> GetTeam(long teamId);
    Task<List<Team>?> GetTeamsOfUser(long userId);
    Task<List<Player>?> GetPlayersOfTeam(long userId, long teamId);

    Task<List<Team>> CreateTeam(TeamCreateDto team);
    Task<Team?> UpdateTeam(long teamId, Team team);
    Task<Team?> UpdateTeamName(long userId, long teamId, string teamName);
    Task<List<Team>> DeleteTeam(long teamId);
    Task<Team?> UserDeleteTeam(long userId, long teamId);

    Task<Team?> AddPlayerToTeam(long userId, long teamId, long playerId);

    Task<List<Team>?> AddTeamToUser(long userId, TeamCreateDto team);
}
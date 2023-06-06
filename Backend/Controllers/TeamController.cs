using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DTOs;
using Backend.Model;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/teams")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet("getAllTeams")]
        [Authorize(Roles = "0")]
        public async Task<List<Team>> GetAllTeams()
        {
            return await _teamService.GetAllTeams();
        }

        [HttpGet("{teamId}")]
        public async Task<Team> GetTeam(long teamId)
        {
            return await _teamService.GetTeam(teamId);
        }

        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<List<Team>> GetTeamsOfUser(long userId)
        {
            return await _teamService.GetTeamsOfUser(userId);
        }

        [HttpGet("user/{userId}/{teamId}")]
        public async Task<List<Player>> GetPlayersOfTeam(long userId, long teamId)
        {
            return await _teamService.GetPlayersOfTeam(userId, teamId);
        }


        [HttpPost("addTeam")]
        public async Task<Team> CreateTeam([FromBody] TeamCreateDto team)
        {
            return await _teamService.CreateTeam(team);
        }

        [HttpPost("addTeam/{userId}")]
        public async Task<List<Team>> CreateTeamOfUser(long userId, TeamCreateDto team)
        {
           return await _teamService.AddTeamToUser(userId, team);
        }

        [HttpPut("updateTeam/{teamId}")]
        public async Task<Team> UpdateTeam(long teamId, [FromBody] Team team)
        {
            return await _teamService.UpdateTeam(teamId, team);
        }

        [HttpPut("addPlayerToTeam/{userId}/{teamId}/{playerId}")]
        public async Task<Team> AddPlayer(long userId,long teamId, long playerId)
        {
            return await _teamService.AddPlayerToTeam(userId,teamId, playerId);
        }

        [HttpDelete("delete/{teamId}")]
        public async Task<List<Team>> DeleteTeam(long teamId)
        {
            return await _teamService.DeleteTeam(teamId);
        }
    }
}

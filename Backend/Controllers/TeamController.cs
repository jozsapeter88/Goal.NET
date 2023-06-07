using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.DTOs;
using Backend.Model;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace Backend.Controllers
{
    [Route("api/teams")]
    [ApiController]
    [Authorize]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly IUserService _userService;

        public TeamController(ITeamService teamService, IUserService userService)
        {
            _teamService = teamService;
            _userService = userService;

        }

        [HttpGet("getAllTeams")]
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
        [Authorize(Roles = "Operator,Admin")]
        public async Task<List<Team>> GetTeamsOfUserAsOp(long userId)
        {
            return await _teamService.GetTeamsOfUser(userId);
        }
        [HttpGet("user/teams")]
        public async Task<IActionResult> GetTeamsOfUser()
        {
            User currentUser = GetCurrentUser();
            List<Team> teams = await _teamService.GetTeamsOfUser(currentUser.Id);

            if (teams == null || teams.Count == 0)
            {
                return NotFound("No teams found for the current user.");
            }

            return Ok(teams);
        }

        [HttpGet("user/{userId}/{teamId}")]
        public async Task<List<Player>> GetPlayersOfTeam(long userId, long teamId)
        {
            return await _teamService.GetPlayersOfTeam(userId, teamId);
        }


        [HttpPost("addTeam")]
        [Authorize(Roles = "Operator,Admin")]
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

        private User? GetCurrentUser()
        {
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;

                if (identity != null)
                {
                    var userClaims = identity.Claims;

                    return _userService
                        .GetUser(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value).Result;
                }
                return null;
            }
        }
    }
}

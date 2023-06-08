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
        public async Task<ActionResult<List<Team>>> GetAllTeams()
        {
            var result = await _teamService.GetAllTeams();
            if (result == null)
            {
                return NotFound("No teams found.");
            }

            return Ok(result);
        }

        [HttpGet("{teamId}")]
        public async Task<ActionResult<Team>> GetTeam(long teamId)
        {
            var result = await _teamService.GetTeam(teamId);
            if (result == null)
            {
                return NotFound("No team found.");
            }

            return Ok(result);
        }

        [HttpGet("user/getTeams/{userId}")]
        [Authorize(Roles = "Operator,Admin")]
        public async Task<ActionResult<List<Team>>> GetTeamsOfUserAsOp(long userId)
        {
           var result= await _teamService.GetTeamsOfUser(userId);
           if (result == null)
           {
               return NotFound("No teams found for the current user.");
           }

           return Ok(result);
        }
        [HttpGet("user/getTeams")]
        public async Task<ActionResult<List<Team>>> GetTeamsOfUser()
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
            {
                return NotFound("User Not Found");
            }
            var result = await _teamService.GetTeamsOfUser(currentUser.Id);

            if (result == null)
            {
                return NotFound("No teams found for the current user.");
            }
            return Ok(result);
        }

        [HttpGet("user/getPlayersOfTeam/{teamId}")]
        public async Task<ActionResult<List<Team>>> GetPlayersOfTeam( long teamId)
        {
            var user = GetCurrentUser();
            if (user == null) return NotFound("Probably user is not logged in!");
            var result = await _teamService.GetPlayersOfTeam(user.Id, teamId);
            if (result == null)
            {
                return NotFound("No teams or players found for the current user.");
            }
            return Ok(result);

        }


        [HttpPost("addTeam")]
        [Authorize(Roles = "Operator,Admin")]
        public async Task<ActionResult<List<Team>>> CreateTeam([FromBody] TeamCreateDto team)
        {
           var result = await _teamService.CreateTeam(team);
           return Ok(result);
        }

        [HttpPost("user/addTeam")]
        public async Task<ActionResult<List<Team>>> CreateTeamOfUser(long userId, TeamCreateDto team)
        {
            var user = GetCurrentUser();
            if (user == null) return NotFound("Probably user is not logged in!");
            var result = await _teamService.AddTeamToUser(user.Id, team);
            if (result == null)
            {
                return NotFound("User was not found");
            }
            return Ok(result);

        }

        [HttpPut("updateTeam/{teamId}")]
        public async Task<ActionResult<Team>> UpdateTeam(long teamId, [FromBody] Team team)
        {
            var result = await _teamService.UpdateTeam(teamId, team);
            if (result == null)
            {
                return NotFound("Team was not found");
            }

            return Ok(result);
        }

        [HttpPut("user/addPlayerToTeam/{teamId}/{playerId}")]
        public async Task<ActionResult<Team>> AddPlayer(long teamId, long playerId)
        { 
            var user = GetCurrentUser();
            if (user == null) return NotFound("Probably user is not logged in!");
            var result = await _teamService.AddPlayerToTeam(user.Id,teamId, playerId);
            if (result == null)
            {
                return NotFound("User or Team was not found");
            }

            return Ok(result);

        }
        [HttpDelete("user/deleteTeam/{teamId}")]
        public async Task<ActionResult<List<Team>>> UserDeleteTeam(long teamId)
        {
            
            var user = GetCurrentUser();
            if (user == null) return NotFound("Probably user is not logged in!");
            List<Team>? teams = await _teamService.UserDeleteTeam(user.Id,teamId);
            if (teams == null)
            {
                return NotFound("User does not have this team!");
            }

            return Ok(teams);

        }

        [HttpDelete("admin/deleteTeam/{teamId}")]
        public async Task<List<Team>> DeleteTeam(long teamId)
        {
            return await _teamService.DeleteTeam(teamId);
        }

        private User? GetCurrentUser()
        {
            {
                if (HttpContext.User.Identity is not ClaimsIdentity identity) return null;
                var userClaims = identity.Claims;

                var username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
                if (username != null)
                    return _userService
                        .GetUser(username).Result;
                return null;
            }
        }
    }
}

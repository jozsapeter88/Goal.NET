using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DTOs;
using Backend.Enums;
using Backend.Model;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/players")]
    [ApiController]
    [Authorize]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet("getAllPlayers")]
        public async Task<List<PlayerDto>> GetAllPlayers()
        {
            return await _playerService.GetAllPlayers();
        }
        
        [HttpGet("getGoalkeepers")]
        public async Task<List<PlayerDto>> GetGoalkeepers()
        {
            return await _playerService.GetGoalKeepers();
        }
        
        [HttpGet("getDefenders")]
        public async Task<List<PlayerDto>> GetDefenders()
        {
            return await _playerService.GetDefenders();
        }
        
        [HttpGet("getMidfielders")]
        public async Task<List<PlayerDto>> GetMidfielders()
        {
            return await _playerService.GetMidfielders();
        }
        
        [HttpGet("getForwards")]
        public async Task<List<PlayerDto>> GetForwards()
        {
            return await _playerService.GetForwards();
        }

        [HttpGet("getNationalities")]
        public Task<List<string>> GetNationalities()
        {
            var nationalityEnums =  Enum.GetNames(typeof(NationalityEnum)).ToList();
            return Task.FromResult(nationalityEnums);
        }
        
        [HttpGet("getPositions")]
        public Task<List<string>> GetPositions()
        {
            var positionEnums =  Enum.GetNames(typeof(PositionEnum)).ToList();
            return Task.FromResult(positionEnums);
        }
        
        
        [HttpGet("getGender")]
        public Task<List<string>> GetGender()
        {
            var genderEnums =  Enum.GetNames(typeof(GenderEnum)).ToList();
            return Task.FromResult(genderEnums);
        }
        
        

        [HttpPost("admin/createPlayer")]
        [Authorize(Roles = "Operator,Admin")]
        public async Task<Player> CreatePlayer([FromBody] PlayerDto player)
        {
            return await _playerService.CreatePlayerByAdmin(player);
        }
        [HttpDelete("delete/{playerId}")]
        [Authorize(Roles = "Operator,Admin")]
        public async Task<List<PlayerDto>> DeletePlayer(long playerId)
        {
            await _playerService.DeletePlayer(playerId);
            return await _playerService.GetAllPlayers();
        }
    }
}

using Backend.DTOs;
using Backend.Enums;
using Backend.Model;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

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
    public async Task<ActionResult<List<PlayerDto>>> GetAllPlayers()
    {
        var players = await _playerService.GetAllPlayers();
        return Ok(players);
    }

    [HttpGet("getGoalkeepers")]
    [AllowAnonymous]
    public async Task<ActionResult<List<PlayerDto>>> GetGoalkeepers()
    {
        var goalkeepers = await _playerService.GetGoalKeepers();
        return Ok(goalkeepers);
    }

    [HttpGet("getDefenders")]
    [AllowAnonymous]
    public async Task<ActionResult<List<PlayerDto>>> GetDefenders()
    {
        var defenders = await _playerService.GetDefenders();
        return Ok(defenders);
    }

    [HttpGet("getMidfielders")]
    [AllowAnonymous]
    public async Task<ActionResult<List<PlayerDto>>> GetMidfielders()
    {
        var midfielders = await _playerService.GetMidfielders();
        return Ok(midfielders);
    }

    [HttpGet("getForwards")]
    [AllowAnonymous]
    public async Task<ActionResult<List<PlayerDto>>> GetForwards()
    {
        var forwards = await _playerService.GetForwards();
        return Ok(forwards);
    }

    [HttpGet("getNationalities")]
    [AllowAnonymous]
    public ActionResult<List<string>> GetNationalities()
    {
        var nationalityEnums = Enum.GetNames(typeof(NationalityEnum)).ToList();
        return Ok(nationalityEnums);
    }

    [HttpGet("getPositions")]
    [AllowAnonymous]
    public ActionResult<List<string>> GetPositions()
    {
        var positionEnums = Enum.GetNames(typeof(PositionEnum)).ToList();
        return Ok(positionEnums);
    }

    [HttpGet("getGender")]
    [AllowAnonymous]
    public ActionResult<List<string>> GetGender()
    {
        var genderEnums = Enum.GetNames(typeof(GenderEnum)).ToList();
        return Ok(genderEnums);
    }

    [HttpPost("admin/createPlayer")]
    [Authorize(Roles = "Operator,Admin")]
    public async Task<ActionResult<Player>> CreatePlayer([FromBody] PlayerDto player)
    {
        var createdPlayer = await _playerService.CreatePlayerByAdmin(player);
        return Ok(createdPlayer);
    }

    [HttpDelete("delete/{playerId}")]
    [Authorize(Roles = "Operator,Admin")]
    public async Task<ActionResult<List<PlayerDto>>> DeletePlayer(long playerId)
    {
        await _playerService.DeletePlayer(playerId);
        var players = await _playerService.GetAllPlayers();
        return Ok(players);
    }
}
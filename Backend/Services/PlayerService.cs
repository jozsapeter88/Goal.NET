using AutoMapper;
using Backend.DTOs;
using Backend.Enums;
using Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class PlayerService : IPlayerService
{
    private readonly GoalContext _context;
    private readonly IMapper _mapper;

    public PlayerService(GoalContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PlayerDto>> GetAllPlayers()
    {
        var players = await _context.Players.ToListAsync();
        var playerDtos = players.Select(player => _mapper.Map<PlayerDto>(player)).ToList();
        return playerDtos;
    }

    public async Task<Player> CreatePlayerByAdmin(PlayerDto player)
    {
        var newPlayer = _mapper.Map<Player>(player);
        _context.Players.Add(newPlayer);
        await _context.SaveChangesAsync();
        return newPlayer;
    }

    public async Task<List<PlayerDto>> GetGoalKeepers()
    {
        var goalKeepers = await _context.Players.Where(p => p.Position == PositionEnum.Goalkeeper).ToListAsync();
        var playerDtos = goalKeepers.Select(player => _mapper.Map<PlayerDto>(player)).ToList();
        return playerDtos;
    }

    public async Task<List<PlayerDto>> GetForwards()
    {
        var forwards = await _context.Players.Where(p => p.Position == PositionEnum.Forward).ToListAsync();
        var playerDtos = forwards.Select(player => _mapper.Map<PlayerDto>(player)).ToList();
        return playerDtos;
    }

    public async Task<List<PlayerDto>> GetMidfielders()
    {
        var midfielders = await _context.Players.Where(p => p.Position == PositionEnum.Midfielder).ToListAsync();
        var playerDtos = midfielders.Select(player => _mapper.Map<PlayerDto>(player)).ToList();
        return playerDtos;
    }

    public async Task<List<PlayerDto>> GetDefenders()
    {
        var defenders = await _context.Players.Where(p => p.Position == PositionEnum.Defender).ToListAsync();
        var playerDtos = defenders.Select(player => _mapper.Map<PlayerDto>(player)).ToList();
        return playerDtos;
    }

    public async Task<List<Player>> DeletePlayer(long playerId)
    {
        var player = await _context.Players.FindAsync(playerId);
        if (player != null) _context.Players.Remove(player);
        await _context.SaveChangesAsync();
        return await _context.Players.ToListAsync();
    }
}
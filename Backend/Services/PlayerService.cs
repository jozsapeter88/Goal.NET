using Backend.Enums;
using Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class PlayerService: IPlayerService
{
    private readonly GoalContext _context;

    public PlayerService(GoalContext context)
    {
        _context = context;
    }
    public async Task<List<Player>> GetAllPlayers()
    {
        return await _context.Players
            .Include(p => p.Team)
            .ToListAsync();
    }

    public async Task<Player> CreatePlayerByAdmin(Player player)
    {
        var newPlayer = new Player
        {
            Name = player.Name,
            Nationality = player.Nationality,
            Position = player.Position,
            Score = player.Score
        };
        _context.Players.Add(newPlayer);
        await _context.SaveChangesAsync();
        return newPlayer;
    }

    public async Task<List<Player>> GetGoalKeepers()
    {
        return await _context.Players.Where(p => p.Position == PositionEnum.Goalkeeper).ToListAsync();
    }

    public async Task<List<Player>> GetForwards()
    {
        return await _context.Players.Where(p => p.Position == PositionEnum.Forward).ToListAsync();
    }

    public async Task<List<Player>> GetMidfielders()
    {
        return await _context.Players.Where(p => p.Position == PositionEnum.Midfielder).ToListAsync();
    }

    public async Task<List<Player>> GetDefenders()
    {
        return await _context.Players.Where(p => p.Position == PositionEnum.Defender).ToListAsync();
    }

    public async Task<List<Player>> DeletePlayer(long playerId)
    {
        var player = await _context.Players.FindAsync(playerId);
        if (player != null) _context.Players.Remove(player);
        await _context.SaveChangesAsync();
        return await _context.Players.ToListAsync();

    }
}
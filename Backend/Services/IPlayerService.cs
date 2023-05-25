using Backend.DTOs;
using Backend.Model;

namespace Backend.Services;

public interface IPlayerService
{
    Task<List<PlayerDto>> GetAllPlayers();
    Task<Player> CreatePlayerByAdmin(PlayerDto player);
    Task<List<PlayerDto>> GetGoalKeepers();
    Task<List<PlayerDto>> GetForwards();
    Task<List<PlayerDto>> GetMidfielders();
    Task<List<PlayerDto>> GetDefenders();
  
    Task<List<Player>> DeletePlayer(long playerId);


}
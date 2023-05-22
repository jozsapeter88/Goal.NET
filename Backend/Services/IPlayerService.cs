using Backend.Model;

namespace Backend.Services;

public interface IPlayerService
{
    Task<List<Player>> GetAllPlayers();
    Task<Player> CreatePlayerByAdmin();
    Task<List<Player>> GetGoalKeepers();
    Task<List<Player>> GetForwards();
    Task<List<Player>> GetMidfielders();
    Task<List<Player>> GetDefenders();
    Task<List<Player>> DeletePlayer(long playerId);


}
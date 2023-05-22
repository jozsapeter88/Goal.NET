namespace Backend.Model;

public class Team
{
    public string Name { get; set; }
    public Coach Coach { get; set; }
    public Player GoalKeeper { get; set; }
    public List<Player> Defenders { get; set; }
    public List<Player> Midfielders { get; set; }
    public List<Player> Forwards { get; set; }
    public int Overall { get; set; }
    public string Color { get; set; }
}
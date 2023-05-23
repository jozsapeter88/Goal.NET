using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model;

public class Team
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public Coach Coach { get; set; }
    public List<Player> AllPlayers { get; set; }
    public int Overall { get; set; }
    public string Color { get; set; }
}
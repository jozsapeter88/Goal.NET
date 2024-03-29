using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Backend.Model;

public class Team
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Coach? Coach { get; set; }
    public List<Player>? AllPlayers { get; set; }
    public int? Overall { get; set; }
    public string Color { get; set; } = string.Empty;

    [JsonIgnore] public User? User { get; set; }
}
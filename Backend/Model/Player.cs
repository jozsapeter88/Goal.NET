using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Backend.Enums;

namespace Backend.Model;

[Serializable]
public class Player
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public PositionEnum Position { get; set; }
    public NationalityEnum Nationality { get; set; }
    public GenderEnum Gender { get; set; }
    public int Score { get; set; }
    [JsonIgnore] public List<Team>? Team { get; set; }
}
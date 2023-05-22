using System.ComponentModel.DataAnnotations.Schema;
using Backend.Enums;

namespace Backend.Model;

public class Player
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string Name { get; set; }
    public PositionEnum Position { get; set; }
    public NationalityEnum Nationality { get; set; }
    public int Score { get; set; }
    public List<Team>? Team { get; set; }
}
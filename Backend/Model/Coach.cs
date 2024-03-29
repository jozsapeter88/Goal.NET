using System.ComponentModel.DataAnnotations.Schema;
using Backend.Enums;

namespace Backend.Model;

public class Coach
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public NationalityEnum Nationality { get; set; }
    public GenderEnum Gender { get; set; }
    public Team? Team { get; set; }
    public long TeamId { get; set; }
}
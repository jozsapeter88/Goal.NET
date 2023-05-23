using System.ComponentModel.DataAnnotations.Schema;
using Backend.Enums;

namespace Backend.Model;

public class Coach
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long? Id { get; set; }
    public string Name { get; set; }
    public NationalityEnum Nationality { get; set; }
    public Gender Gender { get; set; }
}
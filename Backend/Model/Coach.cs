using Backend.Enums;

namespace Backend.Model;

public class Coach
{
    public string Name { get; set; }
    public NationalityEnum Nationality { get; set; }
    public Gender Gender { get; set; }
}
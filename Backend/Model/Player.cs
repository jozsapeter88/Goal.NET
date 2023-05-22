using Backend.Enums;

namespace Backend.Model;

public class Player
{
    public string Name { get; set; }
    public PositionEnum Position { get; set; }
    public NationalityEnum Nationality { get; set; }
    public int Score { get; set; }
}
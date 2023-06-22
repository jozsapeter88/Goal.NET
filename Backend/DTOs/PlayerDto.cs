using Backend.Enums;

namespace Backend.DTOs;

public class PlayerDto
{
    public string Name { get; set; } = string.Empty;
    public PositionEnum Position { get; set; }
    public NationalityEnum Nationality { get; set; }
    public GenderEnum Gender { get; set; }
    public int Score { get; set; }
}
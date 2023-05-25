using Backend.Model;

namespace Backend.DTOs;

public class TeamCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public User? User { get; set; }
}
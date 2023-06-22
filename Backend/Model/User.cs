using System.ComponentModel.DataAnnotations.Schema;
using Backend.Enums;

namespace Backend.Model;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserLevel UserLevel { get; set; }
    public List<Team>? Teams { get; set; }

    public bool CheckPassword(string pass)
    {
        return Password.Equals(pass);
    }
}
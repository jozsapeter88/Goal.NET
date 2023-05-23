using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }

    public bool CheckPassword(string pass)
    {
        return Password.Equals(pass);
    }
}
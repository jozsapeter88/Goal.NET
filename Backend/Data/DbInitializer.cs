using Backend.Enums;
using Backend.Model;
using Backend.Services;

namespace Backend.Data;

public class DbInitializer
{
    public static void Initialize(GoalContext context, IUserService userService)
    {
        
        if(context.GoalUsers.Any()) return;
        var user = new User()
        {
            UserName = "guest",
            Password = userService.HashPassword("guest"),
            UserLevel = UserLevel.User,
            Points = 1000,
            Teams = new List<Team>()
            {
                new Team()
                {
                    
                    Name = "GuestTeam",
                    Coach = new Coach
                    {
                        
                        Name = "Coach Guest",
                        Nationality = NationalityEnum.Hungary,
                        Gender = GenderEnum.Male
                    },
                    Color = "blue",
                    AllPlayers = new List<Player>()
                    {
                        new Player()
                        {
                            
                            Id = -56,
                            Name = "Dominik Szoboszlai",
                            Gender = GenderEnum.Male,
                            Position = PositionEnum.Midfielder,
                            Score = 90,
                            Nationality = NationalityEnum.Hungary
                        }
                    }
                }
            }
        };
        context.GoalUsers.Add(user);
        if (context.Players.Any()) return;

        var players = new[]
        {
            new()
            {
                Name = "Jordan Pickford", Position = PositionEnum.Goalkeeper,
                Nationality = NationalityEnum.UnitedKingdom, Score = 56
            },
            new Player
            {
                Name = "Marcel Halstenberg", Position = PositionEnum.Defender, Nationality = NationalityEnum.Germany,
                Score = 56
            },
            new Player
            {
                Name = "Aaron Ramsey", Position = PositionEnum.Midfielder, Nationality = NationalityEnum.UnitedKingdom,
                Score = 59
            },
            new Player
            {
                Name = "Davide Calabria", Position = PositionEnum.Defender, Nationality = NationalityEnum.Italy,
                Score = 57
            },
            new Player
            {
                Name = "Wissam Ben Yedder", Position = PositionEnum.Forward, Nationality = NationalityEnum.France,
                Score = 55
            },
            new Player
            {
                Name = "Joaquín Correa", Position = PositionEnum.Forward, Nationality = NationalityEnum.Argentina,
                Score = 58
            },
            new Player
            {
                Name = "Stefan de Vrij", Position = PositionEnum.Defender, Nationality = NationalityEnum.Netherlands,
                Score = 56
            },
            new Player
            {
                Name = "José Giménez", Position = PositionEnum.Defender, Nationality = NationalityEnum.Uruguay,
                Score = 54
            },
            new Player
            {
                Name = "Arthur Melo", Position = PositionEnum.Midfielder, Nationality = NationalityEnum.Brazil,
                Score = 58
            },
            new Player
            {
                Name = "Ryan Bertrand", Position = PositionEnum.Defender, Nationality = NationalityEnum.UnitedKingdom,
                Score = 55
            },
            new Player
            {
                Name = "Fabián Ruiz", Position = PositionEnum.Midfielder, Nationality = NationalityEnum.Spain,
                Score = 59
            },
            new Player
            {
                Name = "Alphonse Areola", Position = PositionEnum.Goalkeeper, Nationality = NationalityEnum.France,
                Score = 63
            },
            new Player
            {
                Name = "Nicolò Barella", Position = PositionEnum.Midfielder, Nationality = NationalityEnum.Italy,
                Score = 63
            },
            new Player
            {
                Name = "Dani Ceballos", Position = PositionEnum.Midfielder, Nationality = NationalityEnum.Spain,
                Score = 61
            },
            new Player
            {
                Name = "Houssem Aouar", Position = PositionEnum.Midfielder, Nationality = NationalityEnum.France,
                Score = 65
            },
            new Player
            {
                Name = "Giovanni Reyna", Position = PositionEnum.Midfielder, Nationality = NationalityEnum.UnitedStates,
                Score = 64
            },
            new Player
            {
                Name = "Ricardo Pereira", Position = PositionEnum.Defender, Nationality = NationalityEnum.Portugal,
                Score = 63
            },
            new Player
            {
                Name = "Rúben Neves", Position = PositionEnum.Midfielder, Nationality = NationalityEnum.Portugal,
                Score = 62
            },
            new Player
            {
                Name = "Matteo Guendouzi", Position = PositionEnum.Midfielder, Nationality = NationalityEnum.France,
                Score = 66
            },
            new Player
            {
                Name = "Lucas Ocampos", Position = PositionEnum.Forward, Nationality = NationalityEnum.Argentina,
                Score = 67
            },
            new Player
            {
                Name = "Sergej Milinković-Savić", Position = PositionEnum.Midfielder,
                Nationality = NationalityEnum.Serbia, Score = 68
            },
            new Player
            {
                Name = "Victor Osimhen", Position = PositionEnum.Forward, Nationality = NationalityEnum.Nigeria,
                Score = 69
            },
            new Player
            {
                Name = "Samir Handanović", Position = PositionEnum.Goalkeeper, Nationality = NationalityEnum.Slovenia,
                Score = 75
            },
            new Player
            {
                Name = "Marcus Rashford", Position = PositionEnum.Forward, Nationality = NationalityEnum.UnitedKingdom,
                Score = 73
            },
            new Player
            {
                Name = "Nabil Fekir", Position = PositionEnum.Forward, Nationality = NationalityEnum.France, Score = 72
            },
            new Player
            {
                Name = "Federico Chiesa", Position = PositionEnum.Forward, Nationality = NationalityEnum.Italy,
                Score = 70
            },
            new Player
            {
                Name = "Saul Niguez", Position = PositionEnum.Midfielder, Nationality = NationalityEnum.Spain,
                Score = 76
            },
            new Player
            {
                Name = "Declan Rice", Position = PositionEnum.Midfielder, Nationality = NationalityEnum.UnitedKingdom,
                Score = 77
            },
            new Player
            {
                Name = "Jonathan Tah", Position = PositionEnum.Defender, Nationality = NationalityEnum.Germany,
                Score = 74
            },
            new Player
            {
                Name = "Serge Aurier", Position = PositionEnum.Defender, Nationality = NationalityEnum.Bolivia,
                Score = 72
            },
            new Player
            {
                Name = "Yusuf Yazıcı", Position = PositionEnum.Midfielder, Nationality = NationalityEnum.Turkey,
                Score = 75
            },
            new Player
            {
                Name = "Tanguy Ndombele", Position = PositionEnum.Midfielder, Nationality = NationalityEnum.France,
                Score = 78
            },
            new Player
            {
                Name = "Dani Olmo", Position = PositionEnum.Midfielder, Nationality = NationalityEnum.Spain, Score = 77
            },
            new Player
            {
                Name = "Jan Oblak", Position = PositionEnum.Goalkeeper, Nationality = NationalityEnum.Slovenia,
                Score = 85
            },
            new Player
            {
                Name = "Sadio Mané", Position = PositionEnum.Forward, Nationality = NationalityEnum.Senegal, Score = 86
            },
            new Player
            {
                Name = "Thiago Alcántara", Position = PositionEnum.Midfielder, Nationality = NationalityEnum.Spain,
                Score = 83
            },
            new Player
            {
                Name = "Keylor Navas", Position = PositionEnum.Goalkeeper, Nationality = NationalityEnum.CostaRica,
                Score = 84
            },
            new Player
            {
                Name = "Toby Alderweireld", Position = PositionEnum.Defender, Nationality = NationalityEnum.Belgium,
                Score = 88
            },
            new Player
            {
                Name = "Alex Sandro", Position = PositionEnum.Defender, Nationality = NationalityEnum.Brazil, Score = 82
            },
            new Player
            {
                Name = "Marquinhos", Position = PositionEnum.Defender, Nationality = NationalityEnum.Brazil, Score = 87
            },
            new Player
            {
                Name = "Georginio Wijnaldum", Position = PositionEnum.Midfielder,
                Nationality = NationalityEnum.Netherlands, Score = 85
            },
            new Player
            {
                Name = "Hakim Ziyech", Position = PositionEnum.Midfielder, Nationality = NationalityEnum.Morocco,
                Score = 83
            },
            new Player
            {
                Name = "Leon Goretzka", Position = PositionEnum.Midfielder, Nationality = NationalityEnum.Germany,
                Score = 88
            },
            new Player
            {
                Name = "Fernando Pacheco", Position = PositionEnum.Goalkeeper, Nationality = NationalityEnum.Spain,
                Score = 89
            },
            new Player
            {
                Name = "Manuel Neuer", Position = PositionEnum.Goalkeeper, Nationality = NationalityEnum.Germany,
                Score = 95
            },
            new Player
            {
                Name = "Lionel Messi", Position = PositionEnum.Forward, Nationality = NationalityEnum.Argentina,
                Score = 98
            },
            new Player
            {
                Name = "Cristiano Ronaldo", Position = PositionEnum.Forward, Nationality = NationalityEnum.Portugal,
                Score = 97
            },
            new Player
            {
                Name = "Neymar Jr.", Position = PositionEnum.Forward, Nationality = NationalityEnum.Brazil, Score = 92
            },
            new Player
            {
                Name = "Robert Lewandowski", Position = PositionEnum.Forward, Nationality = NationalityEnum.Poland,
                Score = 95
            },
            new Player
            {
                Name = "Kevin De Bruyne", Position = PositionEnum.Midfielder, Nationality = NationalityEnum.Belgium,
                Score = 91
            },
            new Player
            {
                Name = "Kylian Mbappé", Position = PositionEnum.Forward, Nationality = NationalityEnum.France,
                Score = 94
            },
            new Player
            {
                Name = "Virgil van Dijk", Position = PositionEnum.Defender, Nationality = NationalityEnum.Netherlands,
                Score = 90
            },
            new Player
            {
                Name = "Mohamed Salah", Position = PositionEnum.Forward, Nationality = NationalityEnum.Egypt, Score = 93
            },
            new Player
            {
                Name = "Harry Kane", Position = PositionEnum.Forward, Nationality = NationalityEnum.UnitedKingdom,
                Score = 96
            },
            new Player
            {
                Name = "Jan Vertonghen", Position = PositionEnum.Defender, Nationality = NationalityEnum.Belgium,
                Score = 89
            }
        };

        foreach (var p in players) context.Players.Add(p);

        if (context.Teams.Any()) return;

        var teams = new[]
        {
            new() { Name = "Team 1", Overall = 90, Color = "Red" },
            new Team { Name = "Team 2", Overall = 80, Color = "Yellow" },
            new Team { Name = "Team 3", Overall = 60, Color = "Blue" }
        };

        context.Teams.AddRange(teams);

        context.SaveChanges();
    }
}
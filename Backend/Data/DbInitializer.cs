using Backend.Enums;
using Backend.Model;

namespace Backend.Data;

public class DbInitializer
{
    public static void Initialize(GoalContext context)
    {
        context.Database.EnsureCreated();

        if (context.Players.Any())
        {
            return;
        }

        var players = new Player[]
        {
            new Player
            {
                Name = "Józsi", Position = PositionEnum.Defender, Nationality = NationalityEnum.Afghanistan, Score = 70
            },
            new Player
            {
                Name = "Gézu", Position = PositionEnum.Goalkeeper, Nationality = NationalityEnum.Honduras, Score = 80
            },
            new Player
            {
                Name = "Jocó", Position = PositionEnum.Forward, Nationality = NationalityEnum.Guatemala, Score = 50
            },
            new Player
            {
                Name = "Rómeo", Position = PositionEnum.Midfielder, Nationality = NationalityEnum.Mali, Score = 90
            },
            new Player
            {
                Name = "Aladár", Position = PositionEnum.Forward, Nationality = NationalityEnum.Georgia, Score = 60
            },
        };

        foreach (Player p in players)
        {
            context.Players.Add(p);
        }
        
        if (context.Teams.Any())
        {
            return;
        }
        
        var teams = new Team[]
        {
            new Team
            {
                Name = "Team1"
            },
            new Team
            {
                Name = "Team2"
            },
            new Team
            {
                Name = "Team3"
            },
        };
        
        foreach (Team t in teams)
        {
            context.Teams.Add(t);
        }

        context.SaveChanges();
    }
}
using Microsoft.EntityFrameworkCore;

namespace Backend.Model;

public class GoalContext : DbContext
{
    public DbSet<Player> Players { get; set; }
    public DbSet<Coach> Coaches { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<User> GoalUsers { get; set; }

    public GoalContext(DbContextOptions<GoalContext> options) : base(options)
    {
    }
    
   /*
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Team>()
            .HasOne(t => t.Coach)
            .WithOne(c => c.Team)
            .HasForeignKey<Team>(t => t.CoachId);
    }
    */

}
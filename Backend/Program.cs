using Backend.Enums;
using Backend.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Design;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GoalContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add services
// builder.Services.AddTransient<IRoomService, RoomService>();
// builder.Services.AddTransient<IPotionService, PotionService>();

// Set up endpoints and middleware
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// Migrate and seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<GoalContext>();
        context.Database.Migrate();
        SeedData.Populate(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred migrating and seeding the database.");
    }
}

app.Run();

public static class SeedData
{
    public static void Populate(GoalContext dbContext)
    {
        var player1 = new Player { Name = "Jani", Position = PositionEnum.Goalkeeper, Nationality = NationalityEnum.Afghanistan, Score = 90};
         dbContext.Players.Add(player1);
         dbContext.SaveChanges();
    }
}
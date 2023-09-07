using System.Text;
using Backend.Data;
using Backend.Model;
using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Environment.IsDevelopment()
    ? "DefaultConnection"
    : "DockerCommandsConnectionString";

Console.WriteLine(connectionString);
builder.Services.AddDbContext<GoalContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(connectionString ?? throw new InvalidOperationException("no connectionString"))));


// Add services
builder.Services.AddControllers();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IPlayerService, PlayerService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ITeamService, TeamService>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddCors();
// Get values of JWT from appsettings.json
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

var key = configuration.GetSection("JWT")["key"];
var issuer = configuration.GetSection("JWT")["Issuer"];
var audience = configuration.GetSection("JWT")["Audience"];

// Add authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
            };
        }
    ).AddCookie();

// Set up endpoints and middleware

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

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<GoalContext>();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    try
    {

        if (context.Database.EnsureCreated())
        {
            context.Database.Migrate();
            DbInitializer.Initialize(context, userService);
        }
     
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred migrating and seeding the database");
    }
}

app.UseStaticFiles();
app.UseRouting();

app.UseCors(o =>
{
        o.WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}"
);
app.Run();
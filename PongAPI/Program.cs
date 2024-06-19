using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=pong.db"));

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/players", async (AppDbContext db, Player player) =>
{
    db.Players.Add(player);
    await db.SaveChangesAsync();
    return Results.Created($"/players/{player.Id}", player);
});

app.MapGet("/players", async (AppDbContext db) =>
    await db.Players.ToListAsync());

app.MapGet("/players/{id}", async (AppDbContext db, int id) =>
    await db.Players.FindAsync(id) is Player player
        ? Results.Ok(player)
        : Results.NotFound());

app.MapPost("/scores", async (AppDbContext db, Score score) =>
{
    db.Scores.Add(score);
    await db.SaveChangesAsync();
    return Results.Created($"/scores/{score.Id}", score);
});

app.MapGet("/scores", async (AppDbContext db) =>
    await db.Scores.Include(s => s.Player).ToListAsync());

app.MapGet("/scores/rank", async (AppDbContext db) =>
{
    var scores = await db.Scores
        .GroupBy(s => s.PlayerId)
        .Select(g => new
        {
            PlayerId = g.Key,
            PlayerName = g.First().Player.Name,
            TotalPoints = g.Sum(s => s.Points)
        })
        .OrderByDescending(g => g.TotalPoints)
        .ToListAsync();

    return Results.Ok(scores);
});

app.Run();

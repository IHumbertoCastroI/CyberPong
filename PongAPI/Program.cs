using CyberPong.PongAPI.Data;
using CyberPong.PongAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuração do serviço de banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=pong.db"));

// Configuração do Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pong API", Version = "v1" });
});

// Configuração de CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Middleware para servir arquivos estáticos e padrões
app.UseStaticFiles();
app.UseDefaultFiles();

// Middleware para CORS
app.UseCors();

// Middleware para Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pong API V1");
});

// Endpoints da API
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

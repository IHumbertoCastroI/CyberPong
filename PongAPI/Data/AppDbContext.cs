using Microsoft.EntityFrameworkCore;
using CyberPong.PongAPI.Models;

namespace CyberPong.PongAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Player> Players { get; set; }
        public DbSet<Score> Scores { get; set; }
    }
}

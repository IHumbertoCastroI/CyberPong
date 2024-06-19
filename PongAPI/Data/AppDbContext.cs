using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Player> Players { get; set; }
    public DbSet<Score> Scores { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>().HasKey(p => p.Id);
        modelBuilder.Entity<Score>().HasKey(s => s.Id);
        modelBuilder.Entity<Score>()
            .HasOne(s => s.Player)
            .WithMany(p => p.Scores)
            .HasForeignKey(s => s.PlayerId);
    }
}

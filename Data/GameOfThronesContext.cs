using Microsoft.EntityFrameworkCore;

public class GameOfThronesContext : DbContext
{
    public DbSet<Character> Characters { get; set; }
    public DbSet<House> Houses { get; set; }

    public GameOfThronesContext() { }
    public GameOfThronesContext(DbContextOptions<GameOfThronesContext> options) : base(options) { }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=alab;Username=postgres;Password=root");
        }
    }


}
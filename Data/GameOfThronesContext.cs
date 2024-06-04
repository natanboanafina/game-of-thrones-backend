using Microsoft.EntityFrameworkCore;

public class GameOfThronesContext : DbContext
{
    public DbSet<Character> Characters { get; set; }
    public DbSet<House> Houses { get; set; }
    public DbSet<Data> Datas { get; set; }
    public DbSet<Dragon> Dragons { get; set; }

    public GameOfThronesContext() { }
    public GameOfThronesContext(DbContextOptions<GameOfThronesContext> options) : base(options) { }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=alab;Username=postgres;Password=root");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Character>()
        .HasOne(c => c.Data)
        .WithMany()
        .HasForeignKey(c => c.DataId);

        modelBuilder.Entity<House>()
        .HasOne(h => h.Data)
        .WithMany()
        .HasForeignKey(h => h.HouseId);

        modelBuilder.Entity<Dragon>()
        .HasOne(d => d.Data)
        .WithMany()
        .HasForeignKey(d => d.DragonId);

        base.OnModelCreating(modelBuilder);
    }


}
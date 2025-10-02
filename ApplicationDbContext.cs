// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Tablolarımızı tanımlıyoruz:
    public DbSet<Yemek> Yemekler { get; set; }
    public DbSet<Menukalemi> Menuler { get; set; }

    // İlişkileri daha detaylı tanımlamak için gerekirse OnModelCreating metodu kullanılır.
}
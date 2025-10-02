using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// Identity desteği için IdentityDbContext'ten miras alıyoruz
public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Yemek> Yemekler { get; set; }
    public DbSet<Menukalemi> Menuler { get; set; }
    
    public DbSet<OgunYemegi> OgunYemekleri { get; set; } // Yeni ara tablo
}
// Program.cs (Kısa versiyon)
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// SQLite Bağlantısını Tanımlama
// 'Data Source=menum.db' ifadesi, veritabanı dosyasının uygulamanın çalıştığı klasörde 'menum.db' adıyla oluşturulacağını söyler.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=menum.db"; 

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// ... Diğer servis eklemeleri (Razor Pages, Controllers vb.)

var app = builder.Build();

// ... Diğer app ayarları

// Uygulama ilk çalıştığında veritabanını oluştur/güncelle (Migration)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate(); // Bu satır SQLite dosyasını oluşturur ve tabloları ekler
}

app.Run();
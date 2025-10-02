using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext ve SQLite bağlantısı
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=menum.db"; 

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// Identity Hizmetleri Ekleme (Kullanıcı giriş sistemi)
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Razor Pages ve Diğer Servisler
builder.Services.AddRazorPages();

var app = builder.Build();

// Uygulama ilk çalıştığında veritabanını oluştur/güncelle (Replit'te manuel komut daha güvenilir)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // dbContext.Database.Migrate(); // Bu satırı terminaldeki 'dotnet ef database update' ile değiştiriyoruz.
}

// Hata Yönetimi
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Identity Middlewares Ekleme
app.UseAuthentication(); 
app.UseAuthorization();  

app.MapRazorPages(); // Identity UI sayfalarını da otomatik ekler

app.Run();
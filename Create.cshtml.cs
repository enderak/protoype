// Pages/Yemekler/Create.cshtml.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
// Data ve Models klasörlerinizdeki sınıfları dahil edin
// using YourAppName.Models; 
// using YourAppName.Data; 

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    // Bağımlılık Enjeksiyonu ile DbContext'i alıyoruz
    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    // Formdan gelen veriyi tutmak için Yemek modelini kullanıyoruz. 
    // [BindProperty] bu modelin form verilerini otomatik almasını sağlar.
    [BindProperty]
    public Yemek Yemek { get; set; }

    // Sayfa ilk yüklendiğinde (GET isteği)
    public IActionResult OnGet()
    {
        return Page(); // Boş formu gösterir
    }

    // Form gönderildiğinde (POST isteği)
    public async Task<IActionResult> OnPostAsync()
    {
        // Model doğrulamasını kontrol et (örn: gerekli alanlar boş mu?)
        if (!ModelState.IsValid)
        {
            return Page(); // Hata varsa formu yeniden göster
        }

        // 1. Yeni Yemek kaydını veritabanı bağlamına ekle
        _context.Yemekler.Add(Yemek);
        
        // 2. Değişiklikleri kaydet (Bu noktada SQLite dosyasına yazılır)
        await _context.SaveChangesAsync();

        // Başarılı kayıttan sonra Yemekler listeleme sayfasına yönlendir (Henüz yapmadık ama yapacağız)
        return RedirectToPage("./Index"); 
    }
}
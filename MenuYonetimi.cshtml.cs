// Pages/Menu/MenuYonetimi.cshtml.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class MenuYonetimiModel : PageModel
{
    private readonly ApplicationDbContext _context;
    
    // Yemeklerin Dropdown listesinde kullanılacak listesi
    public SelectList YemekOptions { get; set; } 

    // Kullanıcının seçtiği tarihi tutar
    [BindProperty(SupportsGet = true)]
    public DateTime SeciliTarih { get; set; } = DateTime.Today;

    // Menüyü formdan almak için kullanılan model
    [BindProperty]
    public Dictionary<string, int?> SecilenMenuler { get; set; } = new Dictionary<string, int?>();

    // Öğün tipleri
    public List<string> Ogunler { get; } = new List<string> { "Sabah", "Öğle", "Akşam", "Gece" };

    public MenuYonetimiModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task OnGetAsync()
    {
        // 1. Tüm yemekleri çek ve Dropdown listesi için hazırla
        var yemekler = await _context.Yemekler.OrderBy(y => y.Ad).ToListAsync();
        
        // İlk seçenek olarak "Seçiniz" veya boş bırakmayı sağlamak için 
        YemekOptions = new SelectList(yemekler, nameof(Yemek.Id), nameof(Yemek.Ad));
        
        // 2. Seçili tarihin mevcut menülerini yükle
        var mevcutMenuler = await _context.Menuler
            .Where(m => m.Tarih.Date == SeciliTarih.Date)
            .ToListAsync();

        // Mevcut menüleri forma önceden doldurmak için ayarlıyoruz
        foreach (var ogun in Ogunler)
        {
            var kayit = mevcutMenuler.FirstOrDefault(m => m.Ogun == ogun);
            // Eğer kayıt varsa YemekId'yi, yoksa null'ı ata
            SecilenMenuler[ogun] = kayit?.YemekId; 
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // 1. Önce, seçilen tarihe ait var olan tüm menü kayıtlarını sil
        var eskiMenuler = await _context.Menuler
            .Where(m => m.Tarih.Date == SeciliTarih.Date)
            .ToListAsync();
        
        _context.Menuler.RemoveRange(eskiMenuler);

        // 2. Yeni seçilen menüleri tek tek kaydet
        foreach (var (ogun, yemekId) in SecilenMenuler)
        {
            // Eğer bir yemek seçilmişse (ID null değilse)
            if (yemekId.HasValue && yemekId.Value > 0)
            {
                var yeniKalem = new Menukalemi
                {
                    Tarih = SeciliTarih,
                    Ogun = ogun,
                    YemekId = yemekId.Value
                };
                _context.Menuler.Add(yeniKalem);
            }
        }

        // 3. Değişiklikleri SQLite'a kaydet
        await _context.SaveChangesAsync();

        TempData["Mesaj"] = $"{SeciliTarih.ToShortDateString()} tarihli menü başarıyla güncellendi!";
        
        // Aynı sayfaya yeniden yönlendir (güncel verileri yüklemesi için)
        return RedirectToPage(new { SeciliTarih = SeciliTarih.ToString("yyyy-MM-dd") }); 
    }
}
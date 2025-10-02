// Pages/Index.cshtml.cs (Kullanıcı Tarafı Ana Sayfa)

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    // Seçili tarihi tutar ve formdan veri almayı destekler
    [BindProperty(SupportsGet = true)]
    public DateTime SeciliTarih { get; set; } = DateTime.Today;

    // Menü kalemlerini (Yemek bilgileri ile birlikte) tutacak liste
    public List<Menukalemi> GunlukMenu { get; set; } = new List<Menukalemi>();

    // Öğün sıralaması için sabit bir liste
    public List<string> OgunSirasi { get; } = new List<string> { "Sabah", "Öğle", "Akşam", "Gece" };

    public async Task OnGetAsync()
    {
        // 1. Seçili tarihe ait menü kalemlerini SQLite'dan çek
        GunlukMenu = await _context.Menuler
            // Sorgu yaparken ilgili Yemek nesnesini de otomatik getir ('JOIN' işlemi gibi)
            .Include(m => m.Yemek) 
            .Where(m => m.Tarih.Date == SeciliTarih.Date)
            .ToListAsync();
    }
    
    // Yardımcı Metot: Öğüne göre menü kalemini bulur
    public Menukalemi? GetOgunYemegi(string ogunAdi)
    {
        return GunlukMenu.FirstOrDefault(m => m.Ogun == ogunAdi);
    }
}
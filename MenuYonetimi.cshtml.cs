// Pages/Menu/MenuYonetimi.cshtml.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization; // Gerekli kütüphane

// Sadece giriş yapan kullanıcıların erişimine izin verir
[Authorize]
public class MenuYonetimiModel : PageModel
{
    private readonly ApplicationDbContext _context;
    
    public SelectList YemekOptions { get; set; } 

    [BindProperty(SupportsGet = true)]
    public DateTime SeciliTarih { get; set; } = DateTime.Today;

    [BindProperty]
    public Dictionary<string, int?> SecilenMenuler { get; set; } = new Dictionary<string, int?>();

    public List<string> Ogunler { get; } = new List<string> { "Sabah", "Öğle", "Akşam", "Gece" };

    public MenuYonetimiModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task OnGetAsync()
    {
        var yemekler = await _context.Yemekler.OrderBy(y => y.Ad).ToListAsync();
        
        YemekOptions = new SelectList(yemekler, nameof(Yemek.Id), nameof(Yemek.Ad));
        
        var mevcutMenuler = await _context.Menuler
            .Where(m => m.Tarih.Date == SeciliTarih.Date)
            .ToListAsync();

        foreach (var ogun in Ogunler)
        {
            var kayit = mevcutMenuler.FirstOrDefault(m => m.Ogun == ogun);
            SecilenMenuler[ogun] = kayit?.YemekId; 
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var eskiMenuler = await _context.Menuler
            .Where(m => m.Tarih.Date == SeciliTarih.Date)
            .ToListAsync();
        
        _context.Menuler.RemoveRange(eskiMenuler);

        foreach (var (ogun, yemekId) in SecilenMenuler)
        {
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

        await _context.SaveChangesAsync();

        TempData["Mesaj"] = $"{SeciliTarih.ToShortDateString()} tarihli menü başarıyla güncellendi!";
        
        return RedirectToPage(new { SeciliTarih = SeciliTarih.ToString("yyyy-MM-dd") }); 
    }
}

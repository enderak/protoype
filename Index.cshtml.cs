using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty(SupportsGet = true)]
    public DateTime SeciliTarih { get; set; } = DateTime.Today;

    public List<Menukalemi> GunlukMenu { get; set; } = new List<Menukalemi>();
    public List<string> OgunSirasi { get; } = new List<string> { "Sabah", "Öğle", "Akşam", "Gece" };

    public async Task OnGetAsync()
    {
        // Ara tabloyu (SecilenYemekler) ve yemeğin kendisini (Yemek) dahil et
        GunlukMenu = await _context.Menuler
            .Include(m => m.SecilenYemekler)
                .ThenInclude(oy => oy.Yemek) 
            .Where(m => m.Tarih.Date == SeciliTarih.Date)
            .ToListAsync();
    }
    
    // Belirli bir öğüne ait tüm yemekleri getirir
    public IEnumerable<Yemek> GetOgunYemekleri(string ogunAdi)
    {
        var menukalemi = GunlukMenu.FirstOrDefault(m => m.Ogun == ogunAdi);
        
        return menukalemi?.SecilenYemekler.Select(oy => oy.Yemek) ?? Enumerable.Empty<Yemek>();
    }
}
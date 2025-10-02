using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

[Authorize]
public class MenuYonetimiModel : PageModel
{
    private readonly ApplicationDbContext _context;
    
    public SelectList YemekOptions { get; set; } 

    [BindProperty(SupportsGet = true)]
    public DateTime SeciliTarih { get; set; } = DateTime.Today;

    [BindProperty]
    public Dictionary<string, List<int>> SecilenYemekIdleri { get; set; } = new Dictionary<string, List<int>>();

    public List<string> Ogunler { get; } = new List<string> { "Sabah", "Öğle", "Akşam", "Gece" };

    public MenuYonetimiModel(ApplicationDbContext context) => _context = context;

    public async Task OnGetAsync()
    {
        var yemekler = await _context.Yemekler.OrderBy(y => y.Ad).ToListAsync();
        
        YemekOptions = new SelectList(yemekler, nameof(Yemek.Id), nameof(Yemek.Ad));
        
        var mevcutMenuler = await _context.Menuler
            .Include(m => m.SecilenYemekler) 
            .Where(m => m.Tarih.Date == SeciliTarih.Date)
            .ToListAsync();

        foreach (var ogun in Ogunler)
        {
            var menukalemi = mevcutMenuler.FirstOrDefault(m => m.Ogun == ogun);
            
            if (menukalemi != null)
            {
                SecilenYemekIdleri[ogun] = menukalemi.SecilenYemekler.Select(oy => oy.YemekId).ToList();
            }
            else
            {
                SecilenYemekIdleri[ogun] = new List<int>();
            }
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var mevcutAnaMenuler = await _context.Menuler
            .Include(m => m.SecilenYemekler)
            .Where(m => m.Tarih.Date == SeciliTarih.Date)
            .ToListAsync();
        
        _context.OgunYemekleri.RemoveRange(mevcutAnaMenuler.SelectMany(m => m.SecilenYemekler));
        _context.Menuler.RemoveRange(mevcutAnaMenuler);
        
        foreach (var (ogun, yemekIdList) in SecilenYemekIdleri)
        {
            if (yemekIdList != null && yemekIdList.Any(id => id > 0))
            {
                var yeniAnaKalem = new Menukalemi
                {
                    Tarih = SeciliTarih,
                    Ogun = ogun
                };
                _context.Menuler.Add(yeniAnaKalem);
                await _context.SaveChangesAsync(); 

                foreach (var yemekId in yemekIdList.Where(id => id > 0))
                {
                    _context.OgunYemekleri.Add(new OgunYemegi 
                    { 
                        MenukalemiId = yeniAnaKalem.Id, 
                        YemekId = yemekId 
                    });
                }
            }
        }

        await _context.SaveChangesAsync();
        TempData["Mesaj"] = $"{SeciliTarih.ToShortDateString()} menüsü başarıyla güncellendi (Çoklu Seçim)!";
        return RedirectToPage(new { SeciliTarih = SeciliTarih.ToString("yyyy-MM-dd") }); 
    }
}
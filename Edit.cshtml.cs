// Pages/Yemekler/Edit.cshtml.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Yemek Yemek { get; set; } = default!;

    // Sayfa ilk yüklendiğinde (GET isteği, ID ile)
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound(); // ID yoksa 404 hatası
        }

        // 1. Verilen ID'ye göre yemeği SQLite'dan oku
        var yemek = await _context.Yemekler.FirstOrDefaultAsync(m => m.Id == id);
        
        if (yemek == null)
        {
            return NotFound(); // Yemek bulunamazsa 404 hatası
        }
        
        Yemek = yemek;
        return Page();
    }

    // Form gönderildiğinde (POST isteği)
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // 2. Veritabanındaki kaydı güncellenmiş nesneyle değiştir
        _context.Attach(Yemek).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync(); // SQLite'a kaydet
        }
        catch (DbUpdateConcurrencyException)
        {
            // Eş zamanlı güncelleme hatalarını yönetme (gelişmiş durum)
            if (!_context.Yemekler.Any(e => e.Id == Yemek.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        // Başarılı düzenlemeden sonra listeleme sayfasına yönlendir
        return RedirectToPage("./Index");
    }
}
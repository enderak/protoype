// Pages/Yemekler/Index.cshtml.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // Gerekli kütüphane

// Sadece giriş yapan kullanıcıların erişimine izin verir
[Authorize] 
public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Yemek> Yemekler { get; set; } = default!; 

    public async Task OnGetAsync()
    {
        Yemekler = await _context.Yemekler.ToListAsync();
    }
    
    public async Task<IActionResult> OnPostDeleteAsync(int? id)
    {
        if (id == null) return NotFound();

        var yemek = await _context.Yemekler.FindAsync(id);

        if (yemek != null)
        {
            _context.Yemekler.Remove(yemek);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}

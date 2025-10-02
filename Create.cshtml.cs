// Pages/Yemekler/Create.cshtml.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // Gerekli kütüphane

// Sadece giriş yapan kullanıcıların erişimine izin verir
[Authorize]
public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Yemek Yemek { get; set; }

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Yemekler.Add(Yemek);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index"); 
    }
}

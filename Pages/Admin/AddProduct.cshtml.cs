using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TiendaVirgenFatima.Models;
using TiendaVirgenFatima.Services;
using System.Threading.Tasks;

namespace TiendaVirgenFatima.Pages.Admin
{
    public class AddProductModel : PageModel
    {
        private readonly ProductService _productService;

        public AddProductModel(ProductService productService)
        {
            _productService = productService;
            Product = new Product(); // Inicializar la propiedad Product
        }

        [BindProperty]
        public Product Product { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _productService.CreateAsync(Product);

            return RedirectToPage("/Index");
        }
    }
}

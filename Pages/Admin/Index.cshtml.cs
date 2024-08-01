using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using TiendaVirgenFatima.Models;
using TiendaVirgenFatima.Services;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TiendaVirgenFatima.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly ProductService _productService;

        public IndexModel(ProductService productService)
        {
            _productService = productService;
            Product = new Product();
            Products = new List<Product>();
        }

        [BindProperty]
        public Product Product { get; set; }

        [BindProperty]
        public IFormFile? ProductImage { get; set; } // Permitir valores NULL

        public List<Product> Products { get; set; }

        public async Task OnGetAsync()
        {
            Products = await _productService.GetProductsAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (ProductImage != null)
            {
                var fileName = $"{Product.Name}_{ProductImage.FileName}";
                var filePath = Path.Combine("wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProductImage.CopyToAsync(stream);
                }

                Product.ImagePath = $"/images/{fileName}";
            }

            await _productService.CreateAsync(Product);
            return RedirectToPage();
        }
    }
}

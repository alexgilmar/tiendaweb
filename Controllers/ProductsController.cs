using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaVirgenFatima.Hubs;
using TiendaVirgenFatima.Models;
using TiendaVirgenFatima.Services;

namespace TiendaVirgenFatima.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly IHubContext<ProductHub> _hubContext;

        public ProductsController(ProductService productService, IHubContext<ProductHub> hubContext)
        {
            _productService = productService;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get() =>
            await _productService.GetProductsAsync();

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        public async Task<ActionResult<Product>> Get(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            await _productService.CreateAsync(product);

            return CreatedAtRoute("GetProduct", new { id = product.Id.ToString() }, product);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody] Product productIn)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            productIn.Id = product.Id; // Ensure the ID is not changed
            await _productService.UpdateAsync(id, productIn);

            return NoContent();
        }

        [HttpPut("{id:length(24)}/stock")]
        public async Task<IActionResult> SetStock(string id, [FromBody] int newStock)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var result = await _productService.SetStockAsync(id, newStock);
            if (result)
            {
                await _hubContext.Clients.All.SendAsync("UpdateStock", id, newStock);
            }

            return result ? NoContent() : StatusCode(500);
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            await _productService.RemoveAsync(product.Id);

            return NoContent();
        }
    }
}

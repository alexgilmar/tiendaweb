using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TiendaVirgenFatima.Services;

namespace TiendaVirgenFatima.Hubs
{
    public class ProductHub : Hub
    {
        private readonly ProductService _productService;

        public ProductHub(ProductService productService)
        {
            _productService = productService;
        }

        public async Task ConfirmPurchase(string productId, int quantity)
        {
            if (await _productService.UpdateStockAsync(productId, quantity))
            {
                var product = await _productService.GetProductByIdAsync(productId);
                await Clients.All.SendAsync("UpdateStock", productId, product.Stock);
            }
        }

        public async Task UpdateStockFromAdmin(string productId, int newStock)
        {
            if (await _productService.SetStockAsync(productId, newStock))
            {
                var product = await _productService.GetProductByIdAsync(productId);
                await Clients.All.SendAsync("UpdateStock", productId, product.Stock);
            }
        }
    }
}

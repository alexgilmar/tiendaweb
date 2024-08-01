using MongoDB.Driver;
using TiendaVirgenFatima.Models;
using TiendaVirgenFatima.Settings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TiendaVirgenFatima.Services
{
    public class ProductService
    {
        private readonly IMongoCollection<Product> _products;

        public ProductService(IMongoDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _products = database.GetCollection<Product>(settings.ProductCollectionName);
        }

        public async Task<List<Product>> GetProductsAsync() =>
            await _products.Find(product => true).ToListAsync();

        public async Task<Product> GetProductByIdAsync(string id) =>
            await _products.Find<Product>(product => product.Id == id).FirstOrDefaultAsync();

        public async Task<Product> CreateAsync(Product product)
        {
            await _products.InsertOneAsync(product);
            return product;
        }

        public async Task UpdateAsync(string id, Product productIn) =>
            await _products.ReplaceOneAsync(product => product.Id == id, productIn);

        public async Task RemoveAsync(string id) =>
            await _products.DeleteOneAsync(product => product.Id == id);

        public async Task<bool> UpdateStockAsync(string productId, int decrement)
        {
            var product = await GetProductByIdAsync(productId);
            if (product == null || product.Stock < decrement)
            {
                return false;
            }

            var update = Builders<Product>.Update.Inc(p => p.Stock, -decrement);
            var result = await _products.UpdateOneAsync(p => p.Id == productId, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> SetStockAsync(string productId, int newStock)
        {
            var update = Builders<Product>.Update.Set(p => p.Stock, newStock);
            var result = await _products.UpdateOneAsync(p => p.Id == productId, update);
            return result.ModifiedCount > 0;
        }
    }
}

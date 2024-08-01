using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TiendaVirgenFatima.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImagePath { get; set; }

        public Product()
        {
            Id = ObjectId.GenerateNewId().ToString();
            Name = string.Empty;
            Description = string.Empty;
            ImagePath = string.Empty;
        }
    }
}

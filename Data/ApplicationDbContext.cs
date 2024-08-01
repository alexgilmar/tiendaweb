using Microsoft.EntityFrameworkCore;
using TiendaVirgenFatima.Models;

namespace TiendaVirgenFatima.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}

global using Microsoft.EntityFrameworkCore;
using Projekt_Sklep.Models;

namespace Projekt_Sklep.Data
{
    public class ShopContext : DbContext
    {
        private ModelBuilder modelBuilder;

        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=DESKTOP-SMPHPSP\\SQLEXPRESS;Database=ShopDatabase;Trusted_Connection=True;TrustServerCertificate=True;");
        }


        public DbSet<User> Users => Set<User>();
        public DbSet<Car> Cars => Set<Car>();
        public DbSet<Order> Orders => Set<Order>();
    }
}

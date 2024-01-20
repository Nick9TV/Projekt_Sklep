global using Microsoft.EntityFrameworkCore;
using Projekt_Sklep.Controllers;
using Projekt_Sklep.Models.Projekt_Sklep.Models;
using System.Reflection.Emit;

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

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CartItem>().HasNoKey();

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ShoppingCart> ShopingCarts { get; set; }

    }
}

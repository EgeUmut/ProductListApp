using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductListApp.Models;

namespace ProductListApp.Context
{
    public class ListAppContext:DbContext
    {
        public ListAppContext(DbContextOptions<ListAppContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Slider> Slider { get; set; }
        public DbSet<AboutUs> AboutUs { get; set; }
        public DbSet<HomeDescription> HomeDescription { get; set; }
        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Cart> Carts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Tablo ve ilişki tanımlamalarını burada yapabilirsiniz

            // Product ve Category arasında bir ilişki tanımlama
            modelBuilder.Entity<ShoppingList>()
            .HasMany(p => p.Products) // shoppingLists, birden çok Products'ye sahip olabilir
            .WithMany(c => c.shoppingLists) // Products, birden çok shoppingLists'e sahip olabilir
            .UsingEntity(j => j.ToTable("ProductShoppingList")); // ProductShoppingList adında bir ilişki tablosu oluştur

            modelBuilder.Entity<ShoppingList>()
            .HasMany(p => p.Carts) // shoppingLists, birden çok Products'ye sahip olabilir
            .WithOne(c => c.shoppingList) // Products, birden çok shoppingLists'e sahip olabilir
            .HasForeignKey(p=>p.ShoppingListId)
            .OnDelete(DeleteBehavior.Cascade);


        }

    }
}

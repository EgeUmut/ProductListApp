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


    }
}

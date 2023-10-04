using System.ComponentModel.DataAnnotations;

namespace ProductListApp.Models
{
    public class Cart
    {
        [Key]
        public int id { get; set; }
        public int ProductId { get; set; }
        public Product? product { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Item Count min value is 0")]
        public int? ItemCount { get; set; }
        public string? Description { get; set; }
        public int ShoppingListId { get; set; }
        public ShoppingList? shoppingList { get; set; }
    }
}

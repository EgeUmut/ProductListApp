namespace ProductListApp.Models
{
    public class ShoppingList
    {
        public int id { get; set; }
        public string Name { get; set; }
        public List<Product>? Products { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public string? Description { get; set; }
        public bool? ShoppingStart { get; set; }
        public bool? ShoppingEnd { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ShoppingStartDate { get; set; }
        public DateTime? ShoppingEndDate { get; set; }
    }
}

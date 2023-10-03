using System.ComponentModel.DataAnnotations;

namespace ProductListApp.Models
{

    public class Product
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Item Count min value is 0")]
        public int? ItemCount { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public string? ImageURL { get; set; }
    }
}

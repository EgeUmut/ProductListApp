using System.ComponentModel.DataAnnotations;

namespace ProductListApp.Models
{
    public class Category
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
    }
}

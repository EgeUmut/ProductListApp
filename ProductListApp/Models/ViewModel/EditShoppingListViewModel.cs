namespace ProductListApp.Models.ViewModel
{
    public class EditShoppingListViewModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ProductViewModel> AllProducts { get; set; }
    }

    public class ProductViewModel
    {
        public int Id { get; set; }
        public int ItemCount { get; set; }
    }
}

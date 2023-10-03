using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProductListApp.Models
{
    public class User
    {
        public int? id { get; set; }
        [DisplayName("Full Name")]
        public string FullName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [DataType(DataType.Password)]
        [Required]
        [MinLength(8, ErrorMessage = "Minimum password length is 8 characters!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Password must include at least one lowercase letter, one uppercase letter, and one number.")]
        public string? Password { get; set; }
        [DisplayName("Password Again")]
        [Compare("Password",ErrorMessage="Passwords do not match!")]
        public string? PasswordAgain { get; set; }
        public string? Claim { get; set; }
        public bool? UserStatus { get; set; }
        public List<ShoppingList>? ShoppingLists { get; set; }
    }
}

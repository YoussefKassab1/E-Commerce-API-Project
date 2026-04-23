using Microsoft.AspNetCore.Identity;

namespace E_Commerce.DAL.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public Cart? Cart { get; set; }
        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}

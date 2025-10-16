using System.ComponentModel.DataAnnotations;

namespace OrderManagerMvc.Models
{
    public class Customer
    {
        public int Id { get; set; }
        
        public string FirstName { get; set; } = string.Empty; // Add this property

        public string LastName { get; set; } = string.Empty;  // Add this property

        [EmailAddress, StringLength(200)]
        public string? Email { get; set; }

        [Phone, StringLength(40)]
        public string? Phone { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}

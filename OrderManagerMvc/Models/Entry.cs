using System.ComponentModel.DataAnnotations;

namespace OrderManagerMvc.Models
{
    public class Entry
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Optional: navigation property to related OrderItems
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}

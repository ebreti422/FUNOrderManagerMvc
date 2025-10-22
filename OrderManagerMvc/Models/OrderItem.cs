using System.ComponentModel.DataAnnotations;

namespace OrderManagerMvc.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; } // <-- Make nullable to fix CS8618

        [Range(1, 1000)]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, 10000)]
        public decimal UnitPrice { get; set; }

        public decimal LineTotal => Quantity * UnitPrice;

        // 👇 Add these to fix the foreign key constraint
        public int EntryId { get; set; }
        public Entry? Entry { get; set; } // <-- Make nullable to fix CS8618
    }
}
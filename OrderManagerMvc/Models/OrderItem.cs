using System.ComponentModel.DataAnnotations;

namespace OrderManagerMvc.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        [Range(1, 1000)]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, 10000)]
        public decimal UnitPrice { get; set; } // ? Add this

        public decimal LineTotal => Quantity * UnitPrice; // ? Add this
    }
}
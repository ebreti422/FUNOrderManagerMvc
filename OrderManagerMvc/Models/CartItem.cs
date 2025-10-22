using Microsoft.AspNetCore.Mvc;

namespace OrderManagerMvc.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 1; // Default to 1
        public string? ImageUrl { get; set; }

        public decimal TotalPrice => Price * Quantity;
    }
}
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace OrderManagerMvc.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; } // e.g., Bottles, Jugs, Truckload, Coolers, Softeners
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? PurchaseType { get; set; } // "Buy" or "Rent"
    }
}

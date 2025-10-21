using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace OrderManagerMvc.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; } // Bottles, Jugs, Truckload

        [Required]
        [Range(0.01, 10000)]
        public decimal Price { get; set; } // ? Add this

        public int StockQuantity { get; set; } // ? Add this

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public string Sku { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}

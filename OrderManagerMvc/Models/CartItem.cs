using Microsoft.AspNetCore.Mvc;

namespace OrderManagerMvc.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; } // e.g., Bottles, Truckload, Service, Softener
        public string? PurchaseType { get; set; } // "Buy" or "Rent"
        public decimal Price { get; set; }

        public int Quantity { get; set; } = 1; // For buyable items
        public decimal? Gallons { get; set; } // For truckloads
        public int? RentalMonths { get; set; } // For rentals

        public string? ImageUrl { get; set; }

        // ✅ Smart total price calculation
        public decimal TotalPrice
        {
            get
            {
                if (Type == "Truckload" && Gallons.HasValue)
                {
                    return Price * Gallons.Value;
                }
                else if (PurchaseType == "Rent" && RentalMonths.HasValue)
                {
                    return Price * RentalMonths.Value;
                }
                else
                {
                    return Price * Quantity;
                }
            }
        }
    }
}
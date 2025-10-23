using Microsoft.AspNetCore.Mvc;
using OrderManagerMvc.Models;
using System.Collections.Generic;
using System.Linq;

namespace OrderManagerMvc.Controllers
{
    public class HomeController : Controller
    {
        // Static product list
        private List<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product { Id = 1, Name = "500ml Bottled Water", Type = "Bottles", Price = 1.25M, Description = "Compact and refreshing.", ImageUrl = "/images/BottledWater.png", PurchaseType = "Buy" },
                new Product { Id = 2, Name = "5-Gallon Jug", Type = "Jugs", Price = 8.99M, Description = "Perfect for dispensers.", ImageUrl = "/images/5GallonJug.png", PurchaseType = "Buy" },
                new Product { Id = 3, Name = "Full Truckload", Type = "Truckload", Price = 499.99M, Description = "Bulk delivery for industrial use.", ImageUrl = "/images/WaterTruck.png", PurchaseType = "Buy" },
                new Product { Id = 4, Name = "Water Cooler", Type = "Coolers", Price = 149.99M, Description = "Sleek design with hot/cold options.", ImageUrl = "/images/WaterCooler.png", PurchaseType = "Buy" },
                new Product { Id = 5, Name = "Water Cooler Rental", Type = "Coolers", Price = 19.99M, Description = "Monthly rental with maintenance included.", ImageUrl = "/images/WaterCooler.png", PurchaseType = "Rent" },
                new Product { Id = 6, Name = "Water Softener", Type = "Softeners", Price = 349.99M, Description = "Removes minerals for cleaner water.", ImageUrl = "/images/WaterSoftener.png", PurchaseType = "Buy" },
                new Product { Id = 7, Name = "Water Softener Rental", Type = "Softeners", Price = 29.99M, Description = "Affordable monthly rental.", ImageUrl = "/images/WaterSoftener.png", PurchaseType = "Rent" }
            };
        }

        public IActionResult Index() => View();
        public IActionResult WhySkyDrop() => View();
        public IActionResult Contact() => View();

        public IActionResult Products(string typeFilter, decimal? maxPrice, string purchaseType)
        {
            var products = GetProducts();

            if (!string.IsNullOrEmpty(typeFilter))
                products = products.Where(p => p.Type == typeFilter).ToList();

            if (maxPrice.HasValue)
                products = products.Where(p => p.Price <= maxPrice.Value).ToList();

            if (!string.IsNullOrEmpty(purchaseType))
                products = products.Where(p => p.PurchaseType == purchaseType).ToList();

            ViewBag.TypeFilter = typeFilter;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.PurchaseType = purchaseType;

            return View(products);
        }

        public IActionResult ProductDetails(int id)
        {
            var product = GetProducts().FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();

            return View(product);
        }
    }
}
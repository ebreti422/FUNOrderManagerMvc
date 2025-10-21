using Microsoft.AspNetCore.Mvc;
using OrderManagerMvc.Models;
using System.Collections.Generic;
using System.Linq;
public class HomeController : Controller
{
    private List<Product> GetProducts()
    {
        return new List<Product>
        {
            new Product { Id = 1, Name = "500ml Bottled Water", Type = "Bottles", Price = 1.25M, Description = "Compact and refreshing.", ImageUrl = "/images/bottled-water.jpg", PurchaseType = "Buy" },
            new Product { Id = 2, Name = "5-Gallon Jug", Type = "Jugs", Price = 8.99M, Description = "Perfect for dispensers.", ImageUrl = "/images/jug-water.jpg", PurchaseType = "Buy" },
            new Product { Id = 3, Name = "Full Truckload", Type = "Truckload", Price = 499.99M, Description = "Bulk delivery for industrial use.", ImageUrl = "/images/truckload-water.jpg", PurchaseType = "Buy" },

            // New Products
            new Product { Id = 4, Name = "Water Cooler", Type = "Coolers", Price = 149.99M, Description = "Sleek design with hot/cold options.", ImageUrl = "/images/water-cooler.jpg", PurchaseType = "Buy" },
            new Product { Id = 5, Name = "Water Cooler Rental", Type = "Coolers", Price = 19.99M, Description = "Monthly rental with maintenance included.", ImageUrl = "/images/water-cooler-rent.jpg", PurchaseType = "Rent" },
            new Product { Id = 6, Name = "Water Softener", Type = "Softeners", Price = 349.99M, Description = "Removes minerals for cleaner water.", ImageUrl = "/images/water-softener.jpg", PurchaseType = "Buy" },
            new Product { Id = 7, Name = "Water Softener Rental", Type = "Softeners", Price = 29.99M, Description = "Affordable monthly rental.", ImageUrl = "/images/water-softener-rent.jpg", PurchaseType = "Rent" }
        };
    }

    public IActionResult Index()
    {
        return View();
    }

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

    public IActionResult WhySkyDro()
    {
        return View();
    }
}

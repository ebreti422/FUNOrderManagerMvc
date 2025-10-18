using Microsoft.AspNetCore.Mvc;
using OrderManagerMvc.Models;

public class HomeController : Controller
{
    private List<Product> GetProducts()
    {
        return new List<Product>
        {
            new Product { Id = 1, Name = "500ml Bottled Water", Type = "Bottles", Price = 1.25M, Description = "Compact and refreshing.", ImageUrl = "/images/bottled-water.jpg" },
            new Product { Id = 2, Name = "5-Gallon Jug", Type = "Jugs", Price = 8.99M, Description = "Perfect for dispensers.", ImageUrl = "/images/jug-water.jpg" },
            new Product { Id = 3, Name = "Full Truckload", Type = "Truckload", Price = 499.99M, Description = "Bulk delivery for industrial use.", ImageUrl = "/images/truckload-water.jpg" }
        };
    }
    public IActionResult Index()
    {
        return RedirectToAction("Products");
    }
    public IActionResult Products(string typeFilter, decimal? maxPrice)
    {
        var products = GetProducts();

        if (!string.IsNullOrEmpty(typeFilter))
            products = products.Where(p => p.Type == typeFilter).ToList();

        if (maxPrice.HasValue)
            products = products.Where(p => p.Price <= maxPrice.Value).ToList();

        ViewBag.TypeFilter = typeFilter;
        ViewBag.MaxPrice = maxPrice;

        return View(products);
    }
}

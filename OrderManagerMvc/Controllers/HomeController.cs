using Microsoft.AspNetCore.Mvc;
using OrderManagerMvc.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json; // <-- CRITICAL: Required for Session serialization

public class HomeController : Controller
{
    // The existing method to fetch your product data
    private List<Product> GetProducts()
    {
   
        return new List<Product>
    {
        // 1. Bottled Water
        new Product {
            Id = 1,
            Name = "500ml Bottled Water",
            Type = "Bottles",
            Price = 1.25M,
            Description = "Compact and refreshing.",
            ImageUrl = "/images/BottledWater.png",
            PurchaseType = "Buy"
        },
        // 2. 5-Gallon Jug
        new Product {
            Id = 2,
            Name = "5-Gallon Jug",
            Type = "Jugs",
            Price = 8.99M,
            Description = "Perfect for dispensers.",
            ImageUrl = "/images/5GallonJug.png",
            PurchaseType = "Buy"
        },
        // 3. Full Truckload (Water Truck)
        new Product {
            Id = 3,
            Name = "Full Truckload",
            Type = "Truckload",
            Price = 499.99M,
            Description = "Bulk delivery for industrial use.",
            ImageUrl = "/images/WaterTruck.png", 
            PurchaseType = "Buy"
        },
        // 4. Water Cooler (Buy)
        new Product {
            Id = 4,
            Name = "Water Cooler",
            Type = "Coolers",
            Price = 149.99M,
            Description = "Sleek design with hot/cold options.",
            ImageUrl = "/images/WaterCooler.png",
            PurchaseType = "Buy"
        },
        // 5. Water Cooler Rental
        new Product {
            Id = 5,
            Name = "Water Cooler Rental",
            Type = "Coolers",
            Price = 19.99M,
            Description = "Monthly rental with maintenance included.",
            ImageUrl = "/images/WaterCooler.png", 
            PurchaseType = "Rent"
        },
        // 6. Water Softener (Buy)
        new Product {
            Id = 6,
            Name = "Water Softener",
            Type = "Softeners",
            Price = 349.99M,
            Description = "Removes minerals for cleaner water.",
            ImageUrl = "/images/WaterSoftener.png",
            PurchaseType = "Buy"
        },
        // 7. Water Softener Rental - Reusing the main Water Softener image
        new Product {
            Id = 7,
            Name = "Water Softener Rental",
            Type = "Softeners",
            Price = 29.99M,
            Description = "Affordable monthly rental.",
            ImageUrl = "/images/WaterSoftener.png",
            PurchaseType = "Rent"
        }
    };
    }

    // Standard Actions
    public IActionResult Index() => View();
    public IActionResult WhySkyDrop() => View();
    public IActionResult Contact() => View();

    // Products Catalog with Filtering
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

    // Product Details
    public IActionResult ProductDetails(int id)
    {
        var product = GetProducts().FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    // START OF MISSING CART ACTIONS

    // Action to handle adding an item to the cart
    [HttpPost]
    public IActionResult AddToCart(int id)
    {
        var productToAdd = GetProducts().FirstOrDefault(p => p.Id == id);
        if (productToAdd == null)
        {
            return NotFound();
        }

        // Retrieve cart from session
        var cartJson = HttpContext.Session.GetString("Cart");
        var cart = cartJson == null
            ? new List<CartItem>()
            : JsonSerializer.Deserialize<List<CartItem>>(cartJson);

        // Check if item exists and update quantity
        var existingItem = cart.FirstOrDefault(item => item.ProductId == id);

        if (existingItem != null)
        {
            existingItem.Quantity++;
        }
        else
        {
            // Add new item
            cart.Add(new CartItem
            {
                ProductId = productToAdd.Id,
                Name = productToAdd.Name,
                Price = productToAdd.Price,
                ImageUrl = productToAdd.ImageUrl
            });
        }

        // Save the updated cart back to the session
        HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));

        // Redirect user to the Cart view
        return RedirectToAction("Cart");
    }

    // Action to display the contents of the cart
    public IActionResult Cart()
    {
        var cartJson = HttpContext.Session.GetString("Cart");
        var cart = cartJson == null
            ? new List<CartItem>()
            : JsonSerializer.Deserialize<List<CartItem>>(cartJson);

        // Note: You must ensure the Cart.cshtml view accepts List<CartItem> as its model
        return View(cart);
    }

    // END OF MISSING CART ACTIONS
}

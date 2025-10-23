using Microsoft.AspNetCore.Mvc;
using OrderManagerMvc.Data;
using OrderManagerMvc.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace OrderManagerMvc.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        // View Cart
        public IActionResult Index()
        {
            var cart = GetCart();
            return View("Index", cart); // Points to /Views/Cart/Index.cshtml
        }

        // Add Product to Cart
        [HttpPost]
        public IActionResult AddToCart(CartItem item)
        {
            var product = _context.Products.Find(item.ProductId);
            if (product == null) return NotFound();

            var cart = GetCart();

            // Match by product and customization (excluding quantity)
            var existingItem = cart.FirstOrDefault(i =>
                i.ProductId == item.ProductId &&
                i.Gallons == item.Gallons &&
                i.RentalMonths == item.RentalMonths);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                decimal finalPrice = product.Price;

                if (product.Type == "Truckload" && item.Gallons.HasValue)
                {
                    finalPrice *= item.Gallons.Value;
                    item.Quantity = 1;
                }
                else if (product.PurchaseType == "Rent" && item.RentalMonths.HasValue)
                {
                    finalPrice *= item.RentalMonths.Value;
                    item.Quantity = 1;
                }
                else
                {
                    finalPrice *= item.Quantity;
                }

                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Type = product.Type,
                    PurchaseType = product.PurchaseType,
                    Price = finalPrice,
                    Quantity = item.Quantity,
                    Gallons = item.Gallons,
                    RentalMonths = item.RentalMonths,
                    ImageUrl = item.ImageUrl,
                });
            }

            SaveCart(cart);
            TempData["Success"] = $"{product.Name} added to cart!";
            return RedirectToAction("Index");
        }

        // Remove item
        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }
            return RedirectToAction("Index");
        }

        // Helpers
        private List<CartItem> GetCart()
        {
            var json = HttpContext.Session.GetString("Cart");
            return string.IsNullOrEmpty(json)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(json) ?? new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            //Console.WriteLine(JsonConvert.SerializeObject(cart));
            var json = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("Cart", json);
        }
    }
}
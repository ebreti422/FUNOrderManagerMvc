using Microsoft.AspNetCore.Mvc;
using OrderManagerMvc.Data;
using OrderManagerMvc.Models;
using Newtonsoft.Json;

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
            return View(cart);
        }

        // Add Product to Cart
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var product = _context.Products.Find(productId);
            if (product == null) return NotFound();

            var cart = GetCart();

            var existingItem = cart.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = quantity,
                    ImageUrl = product.ImageUrl
                });
            }

            SaveCart(cart);
            return RedirectToAction("Index");
        }

        // Remove item (optional)
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

        // Helper: Get cart from session
        private List<CartItem> GetCart()
        {
            var json = HttpContext.Session.GetString("Cart");
            return string.IsNullOrEmpty(json)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(json) ?? new List<CartItem>();
        }

        // Helper: Save cart to session
        private void SaveCart(List<CartItem> cart)
        {
            var json = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("Cart", json);
        }
    }
}

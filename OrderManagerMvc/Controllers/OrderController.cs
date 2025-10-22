using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OrderManagerMvc.Data;
using OrderManagerMvc.Models;
using System.Threading.Tasks;

namespace OrderManagerMvc.Controllers

{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        public OrderController(AppDbContext context)
        {
            _context = context;
        }
        // GET: Order
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders.Include(o => o.Customer).ToListAsync();
            return View(orders);
        }
        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(m => m.Id == id); // Changed from FindAsync to FirstOrDefaultAsync for better null handling
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        // GET: Order/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Customers, "Id", "Name");
            return View();
        }
        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderDate,TotalAmount,CustomerId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Customers, "Id", "Name", order.CustomerId);
            return View(order);
        }
        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Customers, "Id", "Name", order.CustomerId);
            return View(order);
        }
        // POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderDate,TotalAmount,CustomerId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Customers, "Id", "Name", order.CustomerId);
            return View(order);
        }
        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.Id == id); // Changed from FindAsync to FirstOrDefaultAsync for better null handling
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            // Retrieve cart from session
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
            if (cart == null || !cart.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Cart", "Home");
            }

            // Retrieve CustomerId from session
            int? customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
            {
                TempData["Error"] = "You must be signed in to place an order.";
                return RedirectToAction("Create", "Customer");
            }

            // Confirm customer exists
            var customer = await _context.Customers.FindAsync(customerId.Value);
            if (customer == null)
            {
                TempData["Error"] = "Customer not found.";
                return RedirectToAction("Create", "Customer");
            }

            // ? Create a new Entry for this order
            var entry = new Entry
            {
                Description = $"Order entry for customer {customerId}",
                CreatedAt = DateTime.UtcNow
            };
            _context.Entries.Add(entry);
            await _context.SaveChangesAsync(); // Save to get Entry.Id

            // ? Create the order and assign EntryId to each item
            var order = new Order
            {
                CustomerId = customerId.Value,
                OrderDate = DateTime.UtcNow,
                Status = "Submitted",
                Items = cart.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price,
                    EntryId = entry.Id // ? Long-term fix: valid FK
                }).ToList()
            };

            order.TotalAmount = order.Items.Sum(i => i.LineTotal);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Clear cart
            HttpContext.Session.Remove("Cart");

            // Redirect to order history
            return RedirectToAction("History", "Order");
        }
        public async Task<IActionResult> History()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
            {
                return RedirectToAction("Create", "Customer"); // or Login page
            }

            var orders = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }
    }
}

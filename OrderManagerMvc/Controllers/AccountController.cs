using Microsoft.AspNetCore.Mvc;
using OrderManagerMvc.Data;
using OrderManagerMvc.Models; // Assume you have a Customer model here
using System.Collections.Generic;
using System.Linq;

public class AccountController : Controller
{
    private readonly AppDbContext _db; // Replace 'YourDbContext' with your actual DbContext class

    public AccountController(AppDbContext db)
    {
        _db = db;
    }

    // Placeholder for the Sign In/Register Page
    public IActionResult Login(string email, string password)
    {
        var customer = _db.Customers.FirstOrDefault(c => c.Email == email && c.Password == password);
        if (customer != null)
        {
            // Store customer ID in session (or cookie)
            HttpContext.Session.SetInt32("CustomerId", customer.Id);
            HttpContext.Session.SetString("CustomerName", customer.Name ?? "User");

            return RedirectToAction("Index", "Home");
        }

        ViewBag.LoginError = "Invalid email or password.";
        ViewBag.ActiveTab = "login";
        return View("~/Views/Customer/Create.cshtml");

    }

    // Action to handle the POST request when a user submits the registration form
    [HttpPost]
    public IActionResult Register(Customer newCustomer)
    {
        if (ModelState.IsValid)
        {
            // 1. Logic to Save the newCustomer data (to a database, etc.)
            // 2. Logic to sign the user in (setting authentication cookie/session)

            // Redirect to home or a profile page
            return RedirectToAction("Index", "Home");
        }

        // If validation fails, return the user back to the form
        return View("Login", newCustomer);
    }
}

public class YourDbContext
{
    public List<Customer> Customers { get; set; } = new List<Customer>();
    // Your DbContext implementation
}
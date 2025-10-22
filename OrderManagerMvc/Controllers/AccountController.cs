using Microsoft.AspNetCore.Mvc;
using OrderManagerMvc.Models; // Assume you have a Customer model here

public class AccountController : Controller
{
    // Placeholder for the Sign In/Register Page
    public IActionResult Login()
    {
        // This will display the form for new registration or login
        return View();
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
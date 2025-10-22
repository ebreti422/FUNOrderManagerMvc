using Microsoft.EntityFrameworkCore;
using OrderManagerMvc.Data;
using OrderManagerMvc.Models;

// seed data for a water delivery service

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        // Ensures the database is created and any pending migrations are applied
        context.Database.Migrate();

        // Look for any customers.
        if (context.Customers.Any())
        {
            return;  // DB has been seeded
        }

        // --- UPDATED CUSTOMER SEED DATA ---
        // Fields updated: Name (single field), PhoneNumber, Address, and Password (required by model)
        var customers = new Customer[]
        {
            // Note: Password fields are for demonstration; use proper hashing in a real application.
            new Customer
            {
                Name = "John Doe",
                Email = "johndoe@email.com",
                PhoneNumber = "555-1234", // Changed from 'Phone'
                Address = "123 Main St",
                Password = "Password123" // Added Password
            },
            new Customer
            {
                Name = "Jane Smith",
                Email = "janesmith@email.com",
                PhoneNumber = "555-5678", // Changed from 'Phone'
                Address = "456 Oak Ave",
                Password = "Password123" // Added Password
            },
            new Customer
            {
                Name = "Bob Johnson",
                Email = "bobjohnson@email.com",
                PhoneNumber = "555-8765", // Changed from 'Phone'
                Address = "789 Pine Rd",
                Password = "Password123" // Added Password
            }
        };

        foreach (Customer c in customers)
        {
            // IMPORTANT: Since Password is required, you must supply a value here.
            context.Customers.Add(c);
        }
        context.SaveChanges();

        // --- UPDATED PRODUCT SEED DATA ---
        // Using the comprehensive data from HomeController for consistency
        var products = new Product[]
        {
            new Product { Id = 1, Name = "500ml Bottled Water", Type = "Bottles", Price = 1.25M, Description = "Compact and refreshing.", ImageUrl = "/images/bottled-water.jpg", PurchaseType = "Buy" },
            new Product { Id = 2, Name = "5-Gallon Jug", Type = "Jugs", Price = 8.99M, Description = "Perfect for dispensers.", ImageUrl = "/images/jug-water.jpg", PurchaseType = "Buy" },
            new Product { Id = 3, Name = "Full Truckload", Type = "Truckload", Price = 499.99M, Description = "Bulk delivery for industrial use.", ImageUrl = "/images/truckload-water.jpg", PurchaseType = "Buy" },
            new Product { Id = 4, Name = "Water Cooler", Type = "Coolers", Price = 149.99M, Description = "Sleek design with hot/cold options.", ImageUrl = "/images/water-cooler.jpg", PurchaseType = "Buy" },
            new Product { Id = 5, Name = "Water Cooler Rental", Type = "Coolers", Price = 19.99M, Description = "Monthly rental with maintenance included.", ImageUrl = "/images/water-cooler-rent.jpg", PurchaseType = "Rent" },
            new Product { Id = 6, Name = "Water Softener", Type = "Softeners", Price = 349.99M, Description = "Removes minerals for cleaner water.", ImageUrl = "/images/water-softener.jpg", PurchaseType = "Buy" },
            new Product { Id = 7, Name = "Water Softener Rental", Type = "Softeners", Price = 29.99M, Description = "Affordable monthly rental.", ImageUrl = "/images/water-softener-rent.jpg", PurchaseType = "Rent" }
        };

        foreach (Product p in products)
        {
            context.Products.Add(p);
        }
        context.SaveChanges();
    }
}

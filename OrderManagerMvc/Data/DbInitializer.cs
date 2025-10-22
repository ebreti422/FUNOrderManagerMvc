using Microsoft.EntityFrameworkCore;
using OrderManagerMvc.Data;
using OrderManagerMvc.Models;

// seed data for a water delivery service

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.Migrate();

        // Look for any customers.
        if (context.Customers.Any())
        {
            return;   // DB has been seeded
        }
        var customers = new Customer[]
        {
            new Customer{FirstName="John",LastName="Doe",Email="johndoe@email.com", Phone="555-1234",Address="123 Main St"}, 
            new Customer{FirstName="Jane",LastName="Smith",Email="janesmith@email.com", Phone="555-5678",Address="456 Oak Ave"},
            new Customer{FirstName="Bob",LastName="Johnson",Email="bobjohnson@email.com", Phone="555-8765",Address="789 Pine Rd"}
            };
        foreach (Customer c in customers)
        {
            context.Customers.Add(c);
        }
        context.SaveChanges();
        var products = new Product[]
        {
            new Product{Name="Spring Water 5 Gallon", Type="Water", Price=10.00M, Description="Pure spring water in a 5-gallon jug", ImageUrl="spring_water_5g.jpg", PurchaseType="One-Time"},
            new Product{Name="Mineral Water 5 Gallon", Type="Water", Price=12.00M, Description="Refreshing mineral water in a 5-gallon jug", ImageUrl="mineral_water_5g.jpg", PurchaseType="Subscription"},
            new Product{Name="Distilled Water 5 Gallon", Type="Water", Price=9.00M, Description="Clean distilled water in a 5-gallon jug", ImageUrl="distilled_water_5g.jpg", PurchaseType="One-Time"}
        };  
        foreach (Product p in products)
        {
            context.Products.Add(p);
        }
        context.SaveChanges();
    }
}

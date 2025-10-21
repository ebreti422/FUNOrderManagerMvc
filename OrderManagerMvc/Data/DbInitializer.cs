
using OrderManagerMvc.Models;

namespace OrderManagerMvc.Data;

// seed data
public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureCreated();
        // Look for any customers.
        if (context.Customers.Any())
        {
            return;   // DB has been seeded
        }
        var customers = new Customer[]
        {
            new Customer{FirstName="Alice", LastName="Johnson", Email="aj@email.com", Phone="555-1234", Address="123 Main St"},
            new Customer{FirstName="Bob", LastName="Smith", Email="bs@email.com", Phone="555-5678", Address="456 Oak Ave"},
            new Customer{FirstName="Charlie", LastName="Brown", Email="cb@email.com", Phone="555-8765", Address="789 Pine Rd"}
            };
        foreach (Customer c in customers)
            {
            context.Customers.Add(c);
        }
        context.SaveChanges();
        var products = new Product[]
        {
            new Product{Sku="P1001", Name="Widget A", Description="High-quality widget A", Price=19.99M, StockQuantity=100},
            new Product{Sku="P1002", Name="Widget B", Description="Durable widget B", Price=29.99M, StockQuantity=150},
            new Product{Sku="P1003", Name="Gadget C", Description="Innovative gadget C", Price=39.99M, StockQuantity=200}
        };
        foreach (Product p in products)
        {
            context.Products.Add(p);
        }
        context.SaveChanges();
        var orders = new Order[]
        {
            new Order{OrderDate=DateTime.Parse("2023-01-15"), TotalAmount=59.97M, CustomerId=1},
            new Order{OrderDate=DateTime.Parse("2023-02-20"), TotalAmount=29.99M, CustomerId=2},
            new Order{OrderDate=DateTime.Parse("2023-03-05"), TotalAmount=19.99M, CustomerId=3}
        };
        foreach (Order o in orders)
        {
            context.Orders.Add(o);
        }
        context.SaveChanges();
        var orderItems = new OrderItem[]
        {
            new OrderItem{OrderId=1, ProductId=1, Quantity=2, UnitPrice=19.99M},
            new OrderItem{OrderId=1, ProductId=2, Quantity=1, UnitPrice=29.99M},
            new OrderItem{OrderId=2, ProductId=2, Quantity=1, UnitPrice=29.99M},
            new OrderItem{OrderId=3, ProductId=1, Quantity=1, UnitPrice=19.99M}
        };
        foreach (OrderItem oi in orderItems)
        {
            context.OrderItems.Add(oi);
        }
        context.SaveChanges();
    }
}



using Microsoft.EntityFrameworkCore;
using OrderManagerMvc.Data;
using System; // Required for TimeSpan

namespace OrderManagerMvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Database Context Service
            builder.Services.AddDbContextFactory<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // START OF CART/SESSION CONFIGURATION

            // 1. Add Distributed Memory Cache (Required by session services)
            builder.Services.AddDistributedMemoryCache();

            // 2. Add Session Service and configure options
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20); // Session lasts 20 minutes of inactivity
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true; // Makes the session cookie essential for site functionality
            });

            // END OF CART/SESSION CONFIGURATION

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<AppDbContext>();
                DbInitializer.Initialize(context);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // 3. Enable Session middleware (Must be after UseRouting and before UseAuthorization)
            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
using ExpenseMVC.Controllers;
using ExpenseTracker.Model;
using ExpenseTrakerLibrary.Repos;
using Microsoft.EntityFrameworkCore;

namespace ExpenseMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();  
            builder.Services.AddSession();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            builder.Services.AddDbContext<ExpenseTrakerDbContext>(options =>
            {
                options.UseSqlServer(@"server=(localdb)\MSSQLLocalDB; database=ExpenseTrackerDB; integrated security=true");

            });
                    
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            
          

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

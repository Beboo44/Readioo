using Demo.DataAccess.Repositories.UoW;
// --- NEW USING DIRECTIVE FOR AUTHENTICATION ---
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Readioo.Business.Services.Classes;
// ... existing usings ...
using Readioo.Business.Services.Interfaces;
using Readioo.Data.Data.Contexts;
using Readioo.Data.Repositories.Authors;
using Readioo.Data.Repositories.Books;

// ... existing usings ...
using System;
// ----------------------------------------------


namespace Readioo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Existing DbContext Registration
            builder.Services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(builder.Configuration.GetConnectionString("Connstring")));

            // ==========================================================
            // === NEW: Configure Authentication Service (CRITICAL STEP 1) ===
            // ==========================================================

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    // This sets the URL where unauthenticated users will be redirected
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.ExpireTimeSpan = TimeSpan.FromHours(24); // Set cookie expiration
                    options.SlidingExpiration = true;
                });

            // ==========================================================
            // === Dependency Injection Registrations (Your Existing Code) ===
            // ==========================================================

            // 1. Register Repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IBookRepository, BookRepository>();
            builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();

            // 2. Register the Unit of Work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // 3. Register the Business Services
            builder.Services.AddScoped<IUserService, UserService>();

            // ==========================================================

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // ==========================================================
            // === NEW: Authentication Middleware (CRITICAL STEP 2) ===
            // ==========================================================
            app.UseAuthentication(); // Must come BEFORE UseAuthorization
            // ==========================================================

            app.UseAuthorization(); // This line was already present, but it must come AFTER UseAuthentication

            // The default route is now set to the Registration page
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Register}/{id?}");

            app.Run();
        }
    }
}

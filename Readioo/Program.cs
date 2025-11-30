﻿using Demo.DataAccess.Repositories.UoW;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Readioo.Business.Services.Classes;
using Readioo.Business.Services.Interfaces;
using Readioo.Data.Data.Contexts;
using Readioo.Data.Repositories;
using Readioo.Data.Repositories.Authors;
using Readioo.Data.Repositories.Books;
using Readioo.Data.Repositories.Genres;
using Readioo.Data.Repositories.Reviews;
using Readioo.Data.Repositories.Shelfs;
using Readioo.DataAccess.Repositories.Generics;

namespace Readioo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // MVC
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
            // DbContext
            builder.Services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
          
            builder.Services.AddMvc().AddNToastNotifyToastr(new ToastrOptions()
            {
                ProgressBar = true,
                CloseButton = true,
                PreventDuplicates = true,
                PositionClass = ToastPositions.TopRight
            });

            // 🔹 Enable SESSION
            builder.Services.AddSession();
            
            // 🔹 Authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.ExpireTimeSpan = TimeSpan.FromHours(24);
                    options.SlidingExpiration = true;
                });

            // DI
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IBookRepository, BookRepository>();
            builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<IAuthorService, AuthorService>();
            builder.Services.AddScoped<IShelfRepository, ShelfRepository>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
            builder.Services.AddScoped<IShelfService, ShelfService>();
            builder.Services.AddScoped<IGenreService, GenreService>();
            builder.Services.AddScoped<IGenreRepository, GenreRepository>();


            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();  // Automatically applies migrations
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseNToastNotify();

            // 🔹 Authentication BEFORE Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // 🔹 Enable SESSION Middleware
            app.UseSession();

            // 🔹 Correct default route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=login}/{id?}");

            // --- CALL THE SEEDER USING A SCOPE BEFORE RUN ---
            using (var scope = app.Services.CreateScope())
            {
                Readioo.Data.Data.AppInitializer.Seed(app);
            }

            app.Run();
        }
    }
}
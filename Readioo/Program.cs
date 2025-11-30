using Demo.DataAccess.Repositories.UoW;
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

            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();

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

            // 🔹 Authentication Configuration
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.ExpireTimeSpan = TimeSpan.FromHours(24);
                    options.SlidingExpiration = true;
                    options.AccessDeniedPath = "/Account/Login";
                });

            // DI Registrations
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
            builder.Services.AddScoped<IRecommendationService, RecommendationService>();

            var app = builder.Build();

            // 🔹 AUTO-RUN MIGRATIONS AND SEED DATA
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    
                    logger.LogInformation("Starting database migration...");
                    context.Database.Migrate(); // 🔹 This runs pending migrations automatically
                    logger.LogInformation("Database migration completed successfully.");
                    
                    logger.LogInformation("Starting data seeding...");
                    Readioo.Data.Data.AppInitializer.Seed(app);
                    logger.LogInformation("Data seeding completed successfully.");
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                    throw; // Re-throw to prevent app from starting with broken database
                }
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

            app.UseSession();

            // 🔹 DEFAULT ROUTE: Redirect unauthenticated users to Login
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
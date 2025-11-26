using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Readioo.Data.Data.Contexts; // Ensure AppDbContext is here
using Readioo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readioo.Data.Data
{
    public class AppInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                // Resolve the Database Context
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                // CHANGED: Use ILogger<AppDbContext> instead of ILogger<Program>
                // This avoids the error if 'Program' is not public or accessible.
                var logger = serviceScope.ServiceProvider.GetService<ILogger<AppDbContext>>();

                if (context == null)
                {
                    logger?.LogError("AppInitializer: Unable to resolve AppDbContext. Seeding skipped.");
                    return;
                }

                try
                {
                    // Ensure the database is created
                    context.Database.EnsureCreated();

                    // Check if Genres already exist. If yes, skip adding them.
                    if (!context.Genres.Any())
                    {
                        var genres = new List<Genre>
                        {
                            new Genre { GenreName = "Science Fiction", Description = "Speculative fiction based on imagined future scientific or technological advances." },
                            new Genre { GenreName = "Horror", Description = "Fiction intended to scare, unsettle, or horrify the reader." },
                            new Genre { GenreName = "Dystopian", Description = "Stories relating to an imagined state or society where there is great suffering or injustice." },
                            new Genre { GenreName = "Fantasy", Description = "Fiction set in a fictional universe, often inspired by myth and folklore." },
                            new Genre { GenreName = "Thriller", Description = "Fiction having many exciting or suspenseful elements." },
                            new Genre { GenreName = "Mystery", Description = "Fiction dealing with the solution of a crime or the unraveling of secrets." },
                            new Genre { GenreName = "Drama", Description = "Narrative fiction intended to be more serious than humorous in tone." },
                            new Genre { GenreName = "Historical", Description = "Fiction set in the past." },
                            new Genre { GenreName = "Political", Description = "Fiction that critiques or explores political systems." },
                            new Genre { GenreName = "Novel", Description = "A fictitious prose narrative of book length." },
                            new Genre { GenreName = "Biography", Description = "An account of someone's life written by someone else." },
                            new Genre { GenreName = "Philosophy", Description = "Books about the study of the fundamental nature of knowledge, reality, and existence." },
                            new Genre { GenreName = "Crime", Description = "Fiction that deals with crimes, their detection, criminals, and their motives." },
                            new Genre { GenreName = "Short Stories", Description = "A story with a fully developed theme but significantly shorter than a novel." },
                            new Genre { GenreName = "Satire", Description = "The use of humor, irony, exaggeration, or ridicule to expose and criticize vices." }
                        };

                        context.Genres.AddRange(genres);
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    logger?.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }
    }
}
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
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                var logger = serviceScope.ServiceProvider.GetService<ILogger<AppDbContext>>();

                if (context == null)
                {
                    logger?.LogError("AppInitializer: Unable to resolve AppDbContext. Seeding skipped.");
                    return;
                }

                try
                {
                    context.Database.EnsureCreated();

                    if (!context.Genres.Any())
                    {
                        var genres = new List<Genre>
                        {
                            new Genre { GenreName = "Science Fiction", Description = "Explore the limitless possibilities of the universe. From space operas and alien civilizations to cybernetic futures and time travel, these stories ask the ultimate 'what if' questions about technology and humanity." },

                            new Genre { GenreName = "Horror", Description = "Prepare to be terrified. These spine-chilling tales explore the darker side of existence, featuring supernatural entities, psychological torments, and the unknown, designed to keep you on the edge of your seat and awake at night." },

                            new Genre { GenreName = "Dystopian", Description = "Enter worlds where society has collapsed or turned oppressive. These gripping narratives explore survival, rebellion, and the resilience of the human spirit amidst totalitarian regimes, environmental disasters, or post-apocalyptic ruins." },
                            
                            new Genre { GenreName = "Fantasy", Description = "Escape into realms of magic, myth, and adventure. Whether it’s epic quests to save a kingdom, battles with dragons, or subtle magical realism, these stories weave folklore and imagination into unforgettable journeys." },
                            
                            new Genre { GenreName = "Thriller", Description = "Fast-paced and adrenaline-fueled, these books are impossible to put down. Packed with high stakes, dangerous chases, and shocking plot twists, thrillers keep you guessing untill the very last page." },
                            
                            new Genre { GenreName = "Mystery", Description = "Put on your detective hat and solve the puzzle. From classic whodunits and noir investigations to cozy village secrets, these stories revolve around crime, clues, and the thrill of uncovering the truth." },
                            
                            new Genre { GenreName = "Drama", Description = "Dive deep into the complexities of the human experience. These emotionally resonant stories focus on realistic characters, interpersonal conflicts, and the triumphs and tragedies of everyday life." },
                            
                            new Genre { GenreName = "Historical", Description = "Travel back in time to experience different eras. Meticulously researched and atmospherically rich, these novels transport readers to the past, blending real events with fictional lives to bring history to life." },
                            
                            new Genre { GenreName = "Political", Description = "Explore the corridors of power and the mechanisms of governance. These narratives critique societal structures, expose corruption, and delve into the ideologies that shape nations and revolutions." },
                            
                            new Genre { GenreName = "Novel", Description = "Immerse yourself in the art of long-form storytelling. These works of fiction prioritize character development and narrative depth, offering a profound exploration of themes that resonate with the human condition." },
                            
                            new Genre { GenreName = "Biography", Description = "Discover the true stories behind the names. These accounts offer intimate looks into the lives of historical figures, innovators, and cultural icons, providing inspiration and insight through their struggles and achievements." },
                            
                            new Genre { GenreName = "Philosophy", Description = "Challenge your perspective and expand your mind. These works delve into the fundamental nature of knowledge, reality, ethics, and existence, encouraging deep contemplation on the meaning of life." },
                            
                            new Genre { GenreName = "Crime", Description = "Step into the shadowy world of lawbreakers and law enforcement. Focusing on the perpetrators, the victims, and the investigators, these gritty tales explore the motives behind criminal acts and the quest for justice." },
                            
                            new Genre { GenreName = "Short Stories", Description = "Perfect for quick escapes, these collections offer bite-sized narratives that pack a punch. Ranging from experimental vignettes to fully realized tales, they capture distinct moments and emotions in a concise format." },
                            
                            new Genre { GenreName = "Satire", Description = "A sharp wit meets social commentary. Using humor, irony, and exaggeration, these clever stories expose the absurdities of society, politics, and human behavior, making you think while you laugh." }

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
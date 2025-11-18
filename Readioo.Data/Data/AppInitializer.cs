using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Readioo.Data.Data.Contexts;
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
        public static void Seed(IApplicationBuilder application)
        {
            using(var SerScope = application.ApplicationServices.CreateScope())
            {
                var context = SerScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                if (!context.Authors.Any())
                {
                    context.Authors.AddRange(new List<Author>()
                    {
                        new Author()
                        {

                        },
                        new Author()
                        {

                        }
                    });
                }

            }
        }
    }
}

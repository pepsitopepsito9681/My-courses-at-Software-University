using LogisticsSystem.Data;
using LogisticsSystem.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using static LogisticsSystem.Areas.Admin.AdminConstants;

namespace LogisticsSystem.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
           this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);

            SeedCategories(services);
            SeedAdministrator(services);
            SeedUser(services);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<LogisticsSystemDbContext>();

            data.Database.Migrate();
        }

        private static void SeedCategories(IServiceProvider services)
        {
            var data = services.GetRequiredService<LogisticsSystemDbContext>();



            if (data.Kinds.Any())
            {
                return;
            }

            data.Kinds.AddRange(new[]
            {
                new Kind()
                {
                    Name="Games and toys",
                    SubKinds=new[]
                    {
                        new SubKind()
                        {
                            Name="For girls"
                        },
                          new SubKind()
                        {
                            Name="For boys"
                        },
                         new SubKind()
                        {
                            Name="Lego"
                        },
                           new SubKind()
                        {
                            Name="Plush toys"
                        },
                           new SubKind()
                        {
                            Name="Cars, trucks and motorcycles"
                        },
                           new SubKind()
                        {
                            Name="Puzzels"
                        },
                        new SubKind()
                        {
                            Name="Playing cards"

                        },
                         new SubKind()
                        {
                            Name="Pistols"

                        },


                    }
                },
                  new Kind()
                {
                    Name="For the student",
                    SubKinds=new[]
                    {
                        new SubKind()
                        {
                            Name="Notebooks"
                        },
                         new SubKind()
                        {
                            Name="School backpacks and bags"
                        },
                        new SubKind()
                        {
                            Name="Pencilcases"

                        },
                        new SubKind()
                        {
                            Name="Painting"


                        },
                        new SubKind()
                        {
                            Name="Writing tools"

                        },
                        new SubKind()
                        {
                            Name="Sketches and blocks"

                        },
                        new SubKind()
                        {
                            Name="Folders and boxes"

                        },
                         new SubKind()
                        {
                            Name="Bindings"

                        },
                           new SubKind()
                        {
                            Name="Cardboards"

                        },

                    }
                },
                    new Kind()
                {
                    Name="Sport",
                    SubKinds=new[]
                    {
                        new SubKind()
                        {
                            Name="Football"
                        },
                         new SubKind()
                        {
                            Name="Volleyball"
                        },
                           new SubKind()
                        {
                            Name="Federball"
                        },
                           new SubKind()
                        {
                            Name="Boxing"
                        },

                    }
                },
                    new Kind()
                {
                    Name="Gifts",
                    SubKinds=new[]
                    {
                         new SubKind()
                        {
                            Name="Jewelry"
                        },
                        new SubKind()
                        {
                            Name="Purses and travel bags"
                        },
                         new SubKind()
                        {
                            Name="Greeting cards and books"
                        },
                           new SubKind()
                        {
                            Name="Piggy banks"
                        },
                           new SubKind()
                        {
                            Name="Keychains"
                        },

                    }
                },
                    new Kind()
                {
                    Name="Books",
                    SubKinds=new[]
                    {
                         new SubKind()
                        {
                            Name="Books for painting"
                        },
                        new SubKind()
                        {
                            Name="Globuses"
                        },
                         new SubKind()
                        {
                            Name="Еncyclopedias"
                        },
                           new SubKind()
                        {
                            Name="Children's literature"
                        },
                           new SubKind()
                        {
                            Name="Literature for teenagers"
                        },

                    }
                },

            });

            data.SaveChanges();
        }

        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                    {
                        return;
                    }

                    var adminRole = new IdentityRole { Name = AdministratorRoleName };

                    await roleManager.CreateAsync(adminRole);

                    const string adminEmail = "admin@gm.com";

                    const string adminPassword = "admin0021";

                    var user = new User
                    {
                        Email = adminEmail,
                        UserName = adminEmail,
                        FullName = "Admin"

                    };

                    await userManager.CreateAsync(user, adminPassword);

                    await userManager.AddToRoleAsync(user, adminRole.Name);

                })
                .GetAwaiter()
                .GetResult();
        }


        private static void SeedUser(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();

            Task
              .Run(async () =>
              {
                  const string userEmail = "test@user.com";

                  if (await userManager.FindByEmailAsync(userEmail) != null)
                  {
                      return;
                  }

                  const string userPassword = "test123";

                  var user = new User
                  {
                      Email = userEmail,
                      UserName = userEmail,
                      FullName = "TestUser"

                  };

                  await userManager.CreateAsync(user, userPassword);


              })
              .GetAwaiter()
              .GetResult();

        }
    }
}

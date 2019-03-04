namespace ZakaraiMe.Web.Infrastructure.Extensions
{
    using Common;
    using Data.Entities.Implementations;
    using Data.Repositories.Contracts;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class ApplicationBuilderExtension
    {
        /// <summary>
        /// Seeds data in the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseDatabaseMigrations<T>(this IApplicationBuilder app) where T : DbContext
        {
            using (IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<T>().Database.Migrate();

                RoleManager<IdentityRole<int>> roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole<int>>>();
                UserManager<User> userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
                IPictureRepository pictureRepo = serviceScope.ServiceProvider.GetService<IPictureRepository>();
                ICarRepository carRepo = serviceScope.ServiceProvider.GetService<ICarRepository>();

                Task
                    .Run(async () =>
                    {
                        carRepo.SeedMakesAndModels();

                        string[] roles = new[]
                        {
                            CommonConstants.AdministratorRole,
                            CommonConstants.DriverRole
                        };

                        foreach (var role in roles)
                        {
                            bool roleExists = await roleManager.RoleExistsAsync(role);

                            if (!roleExists)
                            {
                                await roleManager.CreateAsync(new IdentityRole<int>
                                {
                                    Name = role
                                });
                            }
                        }

                        // Users picture seed
                        string pictureName = "admin";
                        Picture picture = await pictureRepo.GetByName(pictureName);

                        if (picture == null) // Checks whether the admin picture exists
                        {
                            picture = new Picture
                            {
                                FileName = pictureName
                            };

                            await pictureRepo.InsertIntoDatabaseAsync(picture);
                        }

                        // Car picture seed
                        string carPictureName = "carSeed";
                        Picture carPicture = await pictureRepo.GetByName(carPictureName);

                        if (carPicture == null) // Checks whether the picture exists
                        {
                            carPicture = new Picture
                            {
                                FileName = carPictureName
                            };

                            await pictureRepo.InsertIntoDatabaseAsync(carPicture);
                        }

                        // Admin seed
                        string adminEmail = "admin@admin.com";
                        string adminUsername = "admin";
                        string adminPassword = "admin";

                        User adminUser = await userManager.FindByNameAsync(adminUsername);

                        if (adminUser == null)
                        {
                            User user = new User
                            {
                                FirstName = "Admin",
                                LastName = "Admin",
                                Email = adminEmail,
                                UserName = adminUsername,
                                ProfilePictureFileName = pictureName
                            };

                            await userManager.CreateAsync(user, adminPassword);
                            await userManager.AddToRoleAsync(user, CommonConstants.AdministratorRole);
                        }

                        // Driver seed
                        string driverEmail = "driver@driver.com";
                        string driverUsername = "driver";
                        string driverPassword = "driver";

                        User driverUser = await userManager.FindByNameAsync(driverUsername);

                        if (driverUser == null)
                        {
                            driverUser = new User
                            {
                                FirstName = "Daniel",
                                LastName = "Morales",
                                Email = driverEmail,
                                UserName = driverUsername,
                                ProfilePictureFileName = pictureName,
                                Cars = new List<Car>()
                            };

                            await userManager.CreateAsync(driverUser, driverPassword);
                            await userManager.AddToRoleAsync(driverUser, CommonConstants.DriverRole);
                        }

                        // Car seed
                        if (driverUser.Cars.Count() == 0)
                        {
                            await carRepo.CreateAsync(new Car
                            {
                                Colour = "Червен",
                                ModelId = 23,
                                OwnerId = driverUser.Id,
                                PictureFileName = carPictureName
                            });
                        }

                        // Basic user seed
                        string basicUserEmail = "user@user.com";
                        string basicUserUsername = "user";
                        string basicUserPassword = "user";

                        User basicUser = await userManager.FindByNameAsync(basicUserUsername);

                        if (basicUser == null)
                        {
                            User user = new User
                            {
                                FirstName = "John",
                                LastName = "Doe",
                                Email = basicUserEmail,
                                UserName = basicUserUsername,
                                ProfilePictureFileName = pictureName
                            };

                            await userManager.CreateAsync(user, basicUserPassword);
                        }
                    })
                    .Wait();
            }
            return app;
        }
    }
}
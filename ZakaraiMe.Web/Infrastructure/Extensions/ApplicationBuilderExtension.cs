namespace ZakaraiMe.Web.Infrastructure.Extensions
{
    using Common;
    using Data.Entities.Implementations;
    using Data.Repositories.Contracts;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
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

                        string pictureName = "admin";
                        Picture picture = await pictureRepo.GetByName(pictureName);
                        
                        if(picture == null) // Checks whether the admin picture exists
                        {
                            picture = new Picture
                            {
                                FileName = pictureName
                            };

                            await pictureRepo.InsertIntoDatabaseAsync(picture);
                        }

                        string adminEmail = "admin@admin.com";
                        string adminUsername = "admin";
                        string adminPassword = "admin";

                        string driverEmail = "driver@driver.com";
                        string driverUsername = "driver";
                        string driverPassword = "driver";

                        User adminUser = await userManager.FindByNameAsync(adminEmail);

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
                    })
                    .Wait();
            }            
            return app;
        }
    }
}
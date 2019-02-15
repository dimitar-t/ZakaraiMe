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
        public static IApplicationBuilder UseDatabaseMigrations<T>(this IApplicationBuilder app) where T : DbContext
        {
            using (IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<T>().Database.Migrate();

                RoleManager<IdentityRole<int>> roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole<int>>>();
                UserManager<User> userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
                IPictureRepository pictureRepo = serviceScope.ServiceProvider.GetService<IPictureRepository>();

                Task
                    .Run(async () =>
                    {
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
                        
                        if(picture == null)
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
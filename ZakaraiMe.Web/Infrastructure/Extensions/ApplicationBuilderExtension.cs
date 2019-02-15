namespace ZakaraiMe.Web.Infrastructure.Extensions
{
    using Common;
    using Data.Entities.Implementations;
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

                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole<int>>>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();

                Task
                    .Run(async () =>
                    {
                        var roles = new[]
                        {
                            CommonConstants.AdministratorRole,
                            CommonConstants.DriverRole
                        };

                        foreach (var role in roles)
                        {
                            var roleExists = await roleManager.RoleExistsAsync(role);

                            if (!roleExists)
                            {
                                await roleManager.CreateAsync(new IdentityRole<int>
                                {
                                    Name = role
                                });
                            }
                        }

                        var adminEmail = "admin@admin.com";
                        var adminUsername = "admin";
                        var adminPassword = "admin";
                        //TODO: add profile picture to the admin

                        var adminUser = await userManager.FindByNameAsync(adminEmail);

                        if (adminUser != null) //TODO: change the expression to "==" to enable the admin profile seeding
                        {                      // for now it is disabled until the profile picture logic is implemented because the picture is required
                            var user = new User
                            {
                                Email = adminEmail,
                                UserName = adminUsername
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
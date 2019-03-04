namespace ZakaraiMe.Web.Infrastructure.Extensions
{
    using Data.Entities.Implementations;
    using Data.Repositories.Contracts;
    using Microsoft.Extensions.DependencyInjection;
    using Service.Contracts;
    using System.Linq;
    using System.Reflection;

    public static class ServiceCollcetionExtensions
    {
        /// <summary>
        /// Gets .Data and .Service projects and registers the services in them.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            Assembly dataAssembly = Assembly.GetAssembly(typeof(IBaseRepository<User>));
            Assembly serviceAssembly = Assembly.GetAssembly(typeof(IBaseService<User>));

            RegisterServices(services, dataAssembly);
            RegisterServices(services, serviceAssembly);

            return services;
        }

        private static void RegisterServices(IServiceCollection services, Assembly assembly)
        {
            assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => i.Name == $"I{t.Name}"))
                .Select(t => new
                {
                    Inteface = t.GetInterface($"I{t.Name}"),
                    Implementation = t
                })
                .ToList()
                .ForEach(s => services.AddTransient(s.Inteface, s.Implementation));
        }
    }
}

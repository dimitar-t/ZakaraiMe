
namespace ZakaraiMe.Data.Repositories.Implementations
{
    using Contracts;
    using Entities.Implementations;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class CarRepository : BaseRepository<Car>, ICarRepository
    {
        private IHostingEnvironment hostingEnvironment;
        private string connectionString;
        public const string SeedingQueryPath = "/queries/carMakesAndModels.sql";

        public CarRepository(ZakaraiMeContext context, IHostingEnvironment hostingEnvironment, IConfiguration configuration) : base(context)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void SeedMakesAndModels()
        {
            if(context.Models.Count() == 0)
            {
                string script = File.ReadAllText(hostingEnvironment.WebRootPath + SeedingQueryPath);
                context.Database.ExecuteSqlCommand(script);
            }
        }
    }
}

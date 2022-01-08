using Microsoft.EntityFrameworkCore;
using RESTcarsDatabase.Models;

namespace RESTcarsDatabase.Managers
{
    public class CarContext : DbContext
    {
        public CarContext(DbContextOptions<CarContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("DB_CONN_STRING"));
            optionsBuilder.UseSqlServer(Secrets.ConnectionString);
        }

        public DbSet<Car> Cars { get; set; }
        // another: public DbSet<Owner> Owners { get; set; }

        public DbSet<UsedCar> UsedCars { get; set; }
    }
}

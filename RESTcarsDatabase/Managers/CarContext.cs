using Microsoft.EntityFrameworkCore;
using RESTcarsDatabase.Models;

namespace RESTcarsDatabase.Managers
{
    public class CarContext : DbContext
    {
        public CarContext(DbContextOptions<CarContext> options) : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        // another: public DbSet<Owner> Owners { get; set; }
    }
}

using System.Collections.Generic;
using System.Linq;
using RESTcarsDatabase.Models;

namespace RESTcarsDatabase.Managers
{
    public class UsedCarsManagerEF
    {
        private readonly CarContext _context;

        public UsedCarsManagerEF(CarContext context)
        {
            _context = context;
        }

        public IEnumerable<UsedCar> GetAll(string make = null, string model = null, int? price_gte = null,
            int? price_lte = null)
        {
            IQueryable<UsedCar> selectStatement = _context.UsedCars;
            // https://www.tutorialspoint.com/what-is-the-difference-between-ienumerable-and-iqueryable-in-chash
            if (make != null && make.Trim().Length > 0)
            {
                selectStatement = selectStatement.Where(car => car.Make.StartsWith(make));
            }
            if (model != null && model.Trim().Length > 0)
            {
                selectStatement = selectStatement.Where(car => car.Model == model);
            }
            if (price_gte != null)
            {
                selectStatement = selectStatement.Where(car => car.Price > price_gte);
            }
            if (price_lte != null)
            {
                selectStatement = selectStatement.Where(car => car.Price < price_lte);
            }

            return selectStatement;
        }

        public UsedCar GetById(in int id)
        {
            return _context.UsedCars.Find(id);
        }
    }
}

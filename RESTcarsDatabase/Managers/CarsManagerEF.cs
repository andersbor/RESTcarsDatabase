using Microsoft.EntityFrameworkCore;
using RESTcarsDatabase.Models;
using System.Collections.Generic;
using System.Linq;

namespace RESTcarsDatabase.Managers
{
    public class CarsManagerEF : ICarsManager
    {
        private readonly CarContext _context;

        public CarsManagerEF(CarContext context)
        {
            _context = context;
        }

        public IEnumerable<Car> GetAll(string make = null, string model = null, int? price_gte = null, int? price_lte = null)
        // https://www.moesif.com/blog/technical/api-design/REST-API-Design-Filtering-Sorting-and-Pagination/
        {
            IQueryable<Car> selectStatement = _context.Cars;
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

        public Car GetById(int id)
        {
            return _context.Cars.Find(id);
        }

        public Car Add(Car car)
        {
            try
            {
                _context.Cars.Add(car);
                _context.SaveChanges(); // don't forget to save
                // car.Id us updated by the database: id int identity(1,1)
                return car;
            }
            catch (DbUpdateException ex)
            {
                _context.Cars.Remove(car);
                // exception translation
                throw new CarException(ex.InnerException.Message);
            }
        }

        public Car Update(int id, Car updates)
        {
            try
            {
                Car car = _context.Cars.Find(id);
                if (car == null) return null;
                car.Make = updates.Make;
                car.Model = updates.Model;
                car.Price = updates.Price;
                _context.Entry(car).State = EntityState.Modified;
                _context.SaveChanges();
                return car;
            }
            catch (DbUpdateException ex)
            {
                throw new CarException(updates.Make + " " + updates.Model + " " + updates.Price + "\n" + ex.InnerException.Message);
            }
        }

        public Car Delete(int id)
        {
            Car car = _context.Cars.Find(id);
            if (car == null) return null;
            _context.Cars.Remove(car);
            _context.SaveChanges();
            return car;
        }
    }
}

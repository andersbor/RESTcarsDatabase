using Microsoft.EntityFrameworkCore;
using RESTcarsDatabase.Models;
using System.Collections.Generic;

namespace RESTcarsDatabase.Managers
{
    public class CarsManagerEF : ICarsManager
    {
        private readonly CarContext _context;

        public CarsManagerEF(CarContext context)
        {
            _context = context;
        }

        public IEnumerable<Car> GetAll()
        {
            return _context.Cars;
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
                throw new CarException(updates.Make + " " + updates.Model + " " + updates.Price + "\n" +ex.InnerException.Message);
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

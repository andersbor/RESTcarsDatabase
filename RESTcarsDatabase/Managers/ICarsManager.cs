using RESTcarsDatabase.Models;
using System.Collections.Generic;

namespace RESTcarsDatabase.Managers
{
    public interface ICarsManager
    {
        IEnumerable<Car> GetAll(string make = null, string model = null, int? price_gte = null, int? price_lte = null);
        Car GetById(int id);
        public Car Add(Car car);
        public Car Update(int id, Car updates);
        public Car Delete(int id);
    }
}

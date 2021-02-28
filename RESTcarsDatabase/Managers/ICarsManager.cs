using RESTcarsDatabase.Models;
using System.Collections.Generic;

namespace RESTcarsDatabase.Managers
{
    interface ICarsManager
    {
        IEnumerable<Car> GetAll();
        Car GetById(int id);
        public Car Add(Car car);
        public Car Update(int id, Car updates);
        public Car Delete(int id);
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RESTcarsDatabase.Models;

namespace RESTcarsDatabase.Managers.Tests
{
    [TestClass]
    public class CarsManagerTests
    {
        private readonly CarContext _context;

        public CarsManagerTests()
        {
            DbContextOptionsBuilder<CarContext> options = new DbContextOptionsBuilder<CarContext>();
            options.UseSqlServer(Secrets.ConnectionString);
            _context = new CarContext(options.Options);
        }

        [TestMethod]
        public void TestItAll()
        {
            ICarsManager manager = new CarsManagerSqlClient();
            //ICarsManager manager = new CarsManagerEF(_context);
            List<Car> allCars = manager.GetAll().ToList();

            // Add
            Car data = new Car { Make = "Volvo", Model = "XC40", Price = 123456 };
            Car newCar = manager.Add(data);
            Assert.IsTrue(newCar.Id > 0);
            Assert.AreEqual(data.Make, newCar.Make);
            Assert.AreEqual(data.Model, newCar.Model);
            Assert.AreEqual(data.Price, newCar.Price);

            List<Car> allCars2 = manager.GetAll().ToList();
            Assert.AreEqual(allCars.Count + 1, allCars2.Count);

            Car nullModelData = new Car { Make = null, Model = "XC40", Price = 123456 }; ;
            Assert.ThrowsException<CarException>(() => manager.Add(nullModelData));

            // GetById
            Car carById = manager.GetById(newCar.Id);
            Assert.AreEqual(newCar.Id, carById.Id);
            Assert.AreEqual(newCar.Make, carById.Make);
            Assert.AreEqual(newCar.Model, carById.Model);
            Assert.AreEqual(data.Price, carById.Price);

            Assert.IsNull(manager.GetById(newCar.Id + 1));

            // Update
            Car updates = new Car { Make = "Volvo", Model = "XR60", Price = 919368 };
            int id = newCar.Id;
            Car updatedCar = manager.Update(id, updates);
            Assert.AreEqual(id, updatedCar.Id);
            Assert.AreEqual(updates.Make, updatedCar.Make);
            Assert.AreEqual(updates.Model, updatedCar.Model);
            Assert.AreEqual(updates.Price, updatedCar.Price);

            Assert.IsNull(manager.Update(id+1, updates));
            Assert.ThrowsException<CarException>(() => manager.Update(id, nullModelData));
            
            // Delete
            Car deletedCar = manager.Delete(id);
            Assert.AreEqual(id, deletedCar.Id);
            Assert.AreEqual(updatedCar.Make, deletedCar.Make);
            Assert.AreEqual(updatedCar.Model, deletedCar.Model);
            Assert.AreEqual(updatedCar.Price, deletedCar.Price);
            
            List<Car> allCars3 = manager.GetAll().ToList();
            Assert.AreEqual(allCars.Count, allCars3.Count);

            Assert.IsNull(manager.Delete(id));
        }
    }
}
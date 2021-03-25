using Microsoft.AspNetCore.Mvc;
using RESTcarsDatabase.Managers;
using RESTcarsDatabase.Models;
using System.Collections.Generic;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace RESTcarsDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarsManager _manager;

        public CarsController(CarContext context)
        {
            _manager = new CarsManagerEF(context);
            //_manager = new CarsManagerSqlClient();
        }

        // GET: api/<CarsController>
        [HttpGet]
        [ProducesResponseType(Status200OK)]
        public IEnumerable<Car> Get([FromQuery] string make = null, string model = null, int? price_gte = null, int? price_lte = null)
        {
            return _manager.GetAll(make, model, price_gte, price_lte);
        }

        // GET api/<CarsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        public ActionResult<Car> Get(int id)
        {
            Car car = _manager.GetById(id);
            if (car == null) return NotFound("No car with id: " + id);
            return Ok(car);
        }

        // POST api/<CarsController>
        [HttpPost]
        [ProducesResponseType(Status201Created)]
        [ProducesResponseType(Status400BadRequest)]
        public ActionResult<Car> Post([FromBody] Car newCar)
        {
            try
            {
                Car car = _manager.Add(newCar);
                string uri = Url.RouteUrl(RouteData.Values) + "/" + car.Id;
                return Created(uri, car);
            }
            catch (CarException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<CarsController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status400BadRequest)]
        public ActionResult<Car> Put(int id, [FromBody] Car updates)
        {
            try
            {
                Car car = _manager.Update(id, updates);
                if (car == null) return NotFound("No car with id: " + id);
                return Ok(car);
            }
            catch (CarException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<CarsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        public ActionResult<Car> Delete(int id)
        {
            Car car = _manager.Delete(id);
            if (car == null) return NotFound("No car with id: " + id);
            return Ok(car);
        }
    }
}

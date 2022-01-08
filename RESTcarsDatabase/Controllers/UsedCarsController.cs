using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using RESTcarsDatabase.Managers;
using RESTcarsDatabase.Models;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace RESTcarsDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsedCarsController : ControllerBase
    {
        private readonly UsedCarsManagerEF _manager;

        public UsedCarsController(CarContext context)
        {
            _manager = new UsedCarsManagerEF(context);
        }

        // GET: api/<UsedCarsController>
        [HttpGet]
        [ProducesResponseType(Status200OK)]
        public IEnumerable<UsedCar> Get([FromQuery] string make = null, string model = null, int? price_gte = null, int? price_lte = null)
        {
            return _manager.GetAll(make, model, price_gte, price_lte);
        }

        // GET api/<UsedCarsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        public ActionResult<UsedCar> Get(int id)
        {
            UsedCar car = _manager.GetById(id);
            if (car == null) return NotFound("No car with id: " + id);
            return Ok(car);
        }

        // POST api/<UsedCarsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UsedCarsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsedCarsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projekt_Sklep.Services.CarService;

namespace Projekt_Sklep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService CarService) 
        {
            _carService = CarService;
        }

        [HttpGet("GetAllCars")]
        public async Task<ActionResult<List<Car>>> GetAllCars()
        {
            return await _carService.GetAllCars();
        }
        [HttpGet("GetSingleCar/{id}")]
        public async Task<ActionResult<Car>> GetSingleCar(int id)
        {
            var result = await _carService.GetSingleCar(id);
            if (result == null)
                return NotFound("Nie ma samochodu o takim ID");
            return Ok(result);
        }
        [HttpPost("AddCar")]
        public async Task<ActionResult<List<Car>>> AddCar(Car Car)
        {
            var result = await _carService.AddCar(Car);
            return Ok(result);
        }
        [HttpPut("UpdateCar/{id}")]
        public async Task<ActionResult<List<Car>>> UpdateCar(int id, Car request)
        {
            var result = await _carService.UpdateCar(id, request);
            if (result == null)
                return NotFound("Nie ma samochodu o takim ID");
            return Ok(result);

        }
        [HttpDelete("DeleteCar/{id}")]
        public async Task<ActionResult<List<Car>>> DeleteCar(int id)
        {
            var result = await _carService.DeleteCar(id);
            if (result == null)
                return NotFound("Nie ma samochodu o takim ID");
            return Ok(result);
        }

    }
}

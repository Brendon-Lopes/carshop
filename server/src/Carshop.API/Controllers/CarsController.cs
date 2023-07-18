using Carshop.Application.DTOs;
using Carshop.Application.Interfaces.Car;
using Carshop.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carshop.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
public class CarsController : ControllerBase
{
    private readonly ICarService _carService;

    public CarsController(ICarService carService)
    {
        _carService = carService;
    }

    [AllowAnonymous]
    [HttpGet(Name = "GetAllCarsFilteredAndPaginated")]
    public async Task<IActionResult> GetAllCarsFilteredAndPaginated(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 9,
        [FromQuery] string? name = "",
        [FromQuery] string? brandName = "",
        [FromQuery] bool decrescentOrder = true)
    {
        var cars = await _carService.GetAllCarsFilteredAndPaginated(
            page,
            pageSize,
            name!,
            brandName!,
            decrescentOrder);

        return Ok(cars);
    }

    [HttpPost(Name = "SaveCar")]
    public async Task<IActionResult> Save(CarDTO carDto)
    {
        var car = await _carService.Save(carDto);

        return CreatedAtAction(null, new {id = car.Id}, car);
    }

    [HttpPut("{id}", Name = "UpdateCar")]
    public async Task<IActionResult> Update(Guid id, CarDTO carDto)
    {
        var car = await _carService.Update(id, carDto);

        return Ok(car);
    }
}

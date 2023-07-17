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

    [HttpPost(Name = "SaveCar")]
    public async Task<IActionResult> Save(CarDTO carDto)
    {
        var car = await _carService.Save(carDto);

        return CreatedAtAction(null, new {id = car.Id}, car);
    }
}

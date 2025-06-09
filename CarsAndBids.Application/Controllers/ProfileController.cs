using MediatR;
using Microsoft.AspNetCore.Mvc;
using CarsAndBids.Core.CQRS.Profile;
using CarsAndBids.Core.DTOs;
namespace CarsAndBids.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfileController(IMediator mediator) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var result = await mediator.Send(new GetProfileByIdQuery());
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateProfileCommand request)
    {
        try
        {
            var updated = await mediator.Send(request);
            return Ok(updated);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }
}
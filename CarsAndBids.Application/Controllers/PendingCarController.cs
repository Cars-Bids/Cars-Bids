using CarsAndBids.Core.CQRS.PendingCars;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarsAndBids.API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class PendingCarController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await mediator.Send(new GetAllPendingCarsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var result = await mediator.Send(new GetPendingCarByIdQuery { Id = id });
        return result is null
            ? NotFound()
            : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePendingCarCommand request)
    {
        try
        {
            await mediator.Send(request);
            return Created();
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdatePendingCarCommand request)
    {
        try
        {
            await mediator.Send(request);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteById([FromRoute] int id)
    {
        try
        {
            await mediator.Send(new DeletePendingCarByIdCommand { Id = id });
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using CarsAndBids.Core.CQRS.BodyStyles;

namespace CarsAndBids.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BodyStyleController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await mediator.Send(new GetAllBodyStylesQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var result = await mediator.Send(new GetBodyStyleByIdQuery { Id = id });
        return result is null
            ? NotFound()
            : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBodyStyleCommand request)
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
    public async Task<IActionResult> Update([FromBody] UpdateBodyStyleCommand request)
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
            await mediator.Send(new DeleteBodyStyleByIdCommand { Id = id });
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }
}
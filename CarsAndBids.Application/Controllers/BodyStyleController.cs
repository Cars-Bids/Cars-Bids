using MediatR;
using Microsoft.AspNetCore.Mvc;
using CarsAndBids.Core.CQRS.BodyStyles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace CarsAndBids.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BodyStyleController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await mediator.Send(new GetAllBodyStylesQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        });

        return Ok(result);
    }


    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var result = await mediator.Send(new GetBodyStyleByIdQuery { Id = id });
        return result is null
            ? NotFound()
            : Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
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
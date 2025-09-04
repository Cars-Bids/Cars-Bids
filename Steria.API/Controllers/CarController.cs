using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Steria.Core.CQRS.Cars;

namespace Steria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CarController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await mediator.Send(new GetAllCarsQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var result = await mediator.Send(new GetCarByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateCarCommand request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        request.OwnerId = userId;
        await mediator.Send(request);
        return Created();
    }

    [HttpPut]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Update([FromForm] UpdateCarCommand request)
    {
        await mediator.Send(request);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteById([FromRoute] int id)
    {
        await mediator.Send(new DeleteCarByIdCommand { Id = id });
        return Ok();
    }

    [HttpPost("/uploadImages")]
    [AllowAnonymous]
    public async Task<IActionResult> UploadImages([FromForm] UploadImagesQuery query)
    {
        var res = await mediator.Send(query);
        return Ok(res);
    }
}
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CarsAndBids.Core.CQRS.Auctions;

namespace CarsAndBids.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AuctionsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
    {
        var auctions = await mediator.Send(new GetAllAuctionsQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        });
        return Ok(auctions);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var auction = await mediator.Send(new GetAuctionByIdQuery { Id = id });
        return Ok(auction);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateAuctionCommand request)
    {
        await mediator.Send(request);
        return Created();
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromForm] UpdateAuctionCommand request)
    {
        await mediator.Send(request);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteById([FromRoute] int id)
    {
        await mediator.Send(new DeleteAuctionByIdCommand { Id = id });
        return Ok();
    }
}

using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Steria.Core.CQRS.Auctions;
using System.Security.Claims;

namespace Steria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AuctionsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
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
    [AllowAnonymous]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var auction = await mediator.Send(new GetAuctionByIdQuery { Id = id });
        return Ok(auction);
    }

    [HttpGet("detailed/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetDetailedById([FromRoute] int id)
    {
        int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);
        var auction = await mediator.Send(new GetAuctionDetailedByIdQuery 
        { 
            AuctionId = id,
            UserId = userId
        });
        return Ok(auction);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create([FromForm] CreateAuctionCommand request)
    {
        await mediator.Send(request);
        return Created();
    }

    [HttpPut]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Update([FromForm] UpdateAuctionCommand request)
    {
        await mediator.Send(request);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteById([FromRoute] int id)
    {
        await mediator.Send(new DeleteAuctionByIdCommand { Id = id });
        return Ok();
    }

    [HttpGet("active")]
    [AllowAnonymous]
    public async Task<IActionResult> GetActiveAuctions([FromQuery] int count = 10)
    {
        var auctions = await mediator.Send(new GetActiveAuctionsQuery
        {
            Count = count
        });
        return Ok(auctions);
    }

    [Authorize(Roles = "Manager")]
    [HttpGet("managing/{auctionId}")]
    public async Task<IActionResult> GetAuctionForManager([FromRoute] int auctionId)
    {
        var auction = await mediator.Send(new GetManagingAuctionQuery { AutionId = auctionId });
        return Ok(auction);
    }
}

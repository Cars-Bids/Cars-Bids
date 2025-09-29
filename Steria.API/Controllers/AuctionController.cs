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

    [HttpPut("update-status")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateAuctionStatusCommand request)
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
    
    [HttpPost("add-question")]
    public async Task<IActionResult> AddQuestion([FromBody] AddQuestionCommand request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        request.UserId = userId;
        await mediator.Send(request);
        return Created();
    }

    [HttpPost("add-answer")]
    public async Task<IActionResult> AddAnswer([FromBody] AddAnswerCommand request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        request.UserId = userId;
        await mediator.Send(request);
        return Created();
    }

    [HttpGet("filtered")]
    [AllowAnonymous]
    public async Task<IActionResult> GetFilteredAuctions(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? makeModelSearch = null,
        [FromQuery] string? transmission = null,
        [FromQuery] string? bodyStyle = null,
        [FromQuery] int? minMileage = null,
        [FromQuery] int? maxMileage = null,
        [FromQuery] string? sortBy = "CreatedAt",
        [FromQuery] bool sortDescending = true)
    {
        var query = new GetFilteredAuctionsQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            MakeModelSearch = makeModelSearch,
            Transmission = transmission,
            BodyStyle = bodyStyle,
            MinMileage = minMileage,
            MaxMileage = maxMileage,
            SortBy = sortBy,
            SortDescending = sortDescending
        };

        var result = await mediator.Send(query);
        return Ok(result);
    }
}

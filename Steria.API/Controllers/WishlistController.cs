using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Steria.Core.Constants;
using Steria.Core.CQRS.Wishlists;
using System.Security.Claims;


namespace Steria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class WishlistController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var result = await mediator.Send(new GetAllWishlistsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var result = await mediator.Send(new GetWishlistByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWishlistCommand request)
    {
        int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);
        request.UserId = userId;
        await mediator.Send(request);
        return Created();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteByAuction([FromBody] DeleteWishlistByAuctionCommand request)
    {
        int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);
        request.UserId = userId;
        await mediator.Send(request);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateWishlistCommand request)
    {
        await mediator.Send(request);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteById([FromRoute] int id)
    {
        await mediator.Send(new DeleteWishlistByIdCommand { Id = id });
        return Ok();
    }

    [HttpPost("saved-search")]
    public async Task<IActionResult> SaveSearch([FromBody] CreateSavedSearchCommand request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        request.UserId = userId;
        await mediator.Send(request);
        return Created();
    }

    [HttpGet("saved-search/exists")]
    public async Task<IActionResult> CheckSavedSearchExists([FromQuery] int makeId, [FromQuery] int? modelId = null)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var exists = await mediator.Send(new CheckSavedSearchExistsQuery
        {
            UserId = userId,
            MakeId = makeId,
            ModelId = modelId
        });
        return Ok(exists);
    }

    [HttpGet("saved-searches")]
    public async Task<IActionResult> GetSavedSearches([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(new GetUserSavedSearchesQuery
        {
            UserId = userId,
            PageNumber = pageNumber,
            PageSize = pageSize
        });
        return Ok(result);
    }

    [HttpDelete("saved-search/{id}")]
    public async Task<IActionResult> DeleteSavedSearch([FromRoute] int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await mediator.Send(new DeleteSavedSearchCommand
        {
            Id = id,
            UserId = userId
        });
        return Ok();
    }

    [HttpGet("filtered")]
    public async Task<IActionResult> GetFilteredWishlists(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool? endingSoon = null,
        [FromQuery] bool? newCars = null,
        [FromQuery] bool? inspected = null)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(new GetFilteredWishlistsQuery
        {
            UserId = userId,
            PageNumber = pageNumber,
            PageSize = pageSize,
            EndingSoon = endingSoon,
            NewCars = newCars,
            Inspected = inspected
        });
        return Ok(result);
    }
}
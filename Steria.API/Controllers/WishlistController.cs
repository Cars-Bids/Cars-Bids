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
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Update([FromBody] UpdateWishlistCommand request)
    {
        await mediator.Send(request);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteById([FromRoute] int id)
    {
        await mediator.Send(new DeleteWishlistByIdCommand { Id = id });
        return Ok();
    }
}
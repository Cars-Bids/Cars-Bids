using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CarsAndBids.Core.CQRS.Profile;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.CQRS.Account;

namespace CarsAndBids.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ProfileController(IMediator mediator) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(new GetProfileByIdQuery { UserId = userId });
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateProfileCommand request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        request.UserId = userId;
        await mediator.Send(request);
        return Ok();
    }

    [HttpGet("bids-and-wins")]
    public async Task<IActionResult> GetBidsAndWins()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(new GetUserBidsAndWinsQuery { UserId = userId });
        return Ok(result);
    }

    [HttpGet("bidded-cars")]
    public async Task<IActionResult> GetBiddedCars([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(new GetUserBiddedCarsQuery
        {
            UserId = userId,
            PageNumber = pageNumber,
            PageSize = pageSize
        });
        return Ok(result);
    }

    [HttpGet("comments/count")]
    public async Task<ActionResult<int>> GetUserCommentsCount(CancellationToken cancellationToken)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var query = new GetUserCommentsCountQuery(userId);
        var count = await mediator.Send(query, cancellationToken);
        return Ok(count);
    }

    [HttpGet("comments")]
    public async Task<ActionResult<PagedResult<UserCommentDto>>> GetUserComments([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var query = new GetUserCommentsQuery(userId, pageNumber, pageSize);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var command = new ChangePasswordCommand
        {
            UserId = userId,
            OldPassword = dto.OldPassword,
            NewPassword = dto.NewPassword
        };
        var result = await mediator.Send(command);
        if (!result)
            return BadRequest("Failed to change password");
        return Ok();
    }
}
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Steria.Core.CQRS.NotificationSettings;
using Steria.Core.CQRS.Profile;
using Steria.Core.DTOs;
using Steria.Core.Interfaces;
using Steria.Core.CQRS.Manager;

namespace Steria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ProfileController(IMediator mediator, ICacheService cacheService) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(new GetProfileByIdQuery { UserId = userId });
        return Ok(result);
    }



    [HttpPut]
    public async Task<IActionResult> Update([FromForm] UpdateProfileCommand request)
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
    
    [HttpPut("notification-settings")]
    public async Task<IActionResult> UpdateNotificationSettings(UpdateUserNotificationSettingsCommand cmd)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        cmd.UserId = userId;

        await mediator.Send(cmd);
        return Ok();
    }

    [HttpGet("in-review-cars")]
    public async Task<ActionResult<PagedResult<CarDto>>> GetInReviewCars([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var query = new GetUserInReviewCarsQuery(userId, pageNumber, pageSize);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("auction-comments")]
    public async Task<ActionResult<PagedResult<UserCommentDto>>> GetUserAuctionComments([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var query = new GetUserAuctionCommentsQuery(userId, pageNumber, pageSize);
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

    [HttpGet("ended-auctions")]
    public async Task<ActionResult<PagedResult<AuctionWithCarDto>>> GetUserEndedAuctions([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var query = new GetUserEndedAuctionsQuery(userId, pageNumber, pageSize);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("wishlist")]
    public async Task<ActionResult<PagedResult<WishlistItemDto>>> GetWishlist(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    CancellationToken cancellationToken = default)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var query = new GetUserWishlistQuery(userId, pageNumber, pageSize);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("active-auctions")]
    public async Task<ActionResult<PagedResult<AuctionWithCarDto>>> GetUserActiveAuctions(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    CancellationToken cancellationToken = default)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var query = new GetUserActiveAuctionsQuery(userId, pageNumber, pageSize);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("user-notification-setting")]
    public async Task<IActionResult> GetUserNotificationSettings()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var settings = await cacheService.GetUserSettingsAsync(userId);
        if (settings == null || !settings.Any())
        {
            return NotFound(new { Message = "No notification settings found for the user." });
        }

        return Ok(settings);

    }
    [HttpGet("in-pending-cars")]
    public async Task<ActionResult<PagedResult<ProfileInReviewCarDto>>> GetInPendingCars([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var query = new GetAllInPendingCarsQuery(pageNumber, pageSize);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("managed-cars")]
    public async Task<ActionResult<PagedResult<AuctionWithCarDto>>> GetManagedCars([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var query = new GetManagedCarsQuery(userId, pageNumber, pageSize);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
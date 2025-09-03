using MediatR;
using Microsoft.AspNetCore.Mvc;
using Steria.Core.CQRS.Comments;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Steria.Core.CQRS.Auctions;
using Steria.Core.Enums;

namespace Steria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CommentController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var result = await mediator.Send(new GetAllCommentsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var result = await mediator.Send(new GetCommentByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateCommentCommand request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        request.UserId = userId;
        await mediator.Send(request);
        return Created();
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] UpdateCommentCommand request)
    {
        await mediator.Send(request);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteById([FromRoute] int id)
    {
        await mediator.Send(new DeleteCommentByIdCommand { Id = id });
        return Ok();
    }
    
    [AllowAnonymous]
    [HttpGet("activity")]
    public async Task<IActionResult> GetAuctionActivity(int auctionId, CommentTabEnum tab, int pageNumber = 1, int pageSize = 10)
    {
        return tab switch
        {
            CommentTabEnum.Newest => Ok(await mediator.Send(new GetNewestActivityQuery { AuctionId = auctionId, PageSize = pageSize, PageNumber = pageNumber })),
            CommentTabEnum.MostUpvoted => Ok(await mediator.Send(new GetMostUpvotedCommentsQuery { AuctionId = auctionId, PageSize = pageSize, PageNumber = pageNumber})),
            CommentTabEnum.SellerComments => Ok(await mediator.Send(new GetAuctionSellerCommentsQuery { AuctionId = auctionId, PageSize = pageSize, PageNumber = pageNumber })),
            CommentTabEnum.BidHistory => Ok(await mediator.Send(new GetAuctionBidsQuery { AuctionId = auctionId, PageSize = pageSize, PageNumber = pageNumber })),
            _ => BadRequest("Invalid tab")
        };
    }
}
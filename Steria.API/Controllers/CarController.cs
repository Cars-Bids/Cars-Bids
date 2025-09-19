using System.Security.Claims;
using CloudinaryDotNet.Actions;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Steria.Core.CQRS.Cars;
using Steria.Core.Enums;

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

    [HttpPost("requestNewCar")]
    public async Task<IActionResult> RequestNewCar([FromForm] RequestNewCarCommand command)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        command.userId = userId;
        await mediator.Send(command);
        return Ok();
    }

    [HttpPost("{carId}/assign-and-start-auction")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> AssignAndStartAuction([FromRoute] int carId)
    {
        var managerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var command = new AssignCarAndCreateAuctionCommand
        {
            CarId = carId,
            ManagerId = managerId
        };
        await mediator.Send(command);
        return Ok();
    }

    [Authorize(Roles = "Manager")]
    [HttpPost("{carId}/images")]
    public async Task<IActionResult> AddImages(int carId, [FromForm] List<IFormFile> files,
        [FromQuery] ImageCategory category)
    {
        var cmd = new AddImagesCommand { CarId = carId, Files = files, ImageCategory = category };
        await mediator.Send(cmd);
        return Ok();
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("{carId}/images/{imageId}")]
    public async Task<IActionResult> DeleteImages(int carId, int imageId)
    {
        var cmd = new DeleteImagesCommand { CarId = carId, ImageId = imageId };
        await mediator.Send(cmd);

        return Ok();
    }

    [Authorize(Roles = "Manager")]
    [HttpPut("{carId}/images/order")]
    public async Task<IActionResult> UpdateOrder(int carId, [FromBody] List<int> orderedImageIds)
    {
        await mediator.Send(new UpdateCarImagesOrderCommand
        {
            CarId = carId,
            OrderedImageIds = orderedImageIds
        });
        return Ok();
    }
}
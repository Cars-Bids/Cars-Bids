using MediatR;
using Microsoft.AspNetCore.Mvc;
using CarsAndBids.Core.CQRS.Categories;

namespace CarsAndBids.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var res = await mediator.Send(new GetAllCategoriesQuery());
        return Ok(res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var res = await mediator.Send(new GetCategoryByIdQuery { Id = id });
        
        return res is null
            ? NotFound()
            : Ok(res);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateCategoryCommand request)
    {
        try
        {
            await mediator.Send(request);
            return Created();
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteById([FromRoute] int id)
    {
        try
        {
            await mediator.Send(new DeleteCategoryByIdCommand { Id = id });
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using WebApiCarsBids.Models.Category;

namespace WebApiCarsBids.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriesController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CategoryCreateModel model)
    {
        return Ok();
    }
}

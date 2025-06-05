using CarsAndBids.Core.Interfaces;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CarsAndBids.Core.CQRS.Categories;

public class CreateCategoryCommand : IRequest
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public IFormFile? Image { get; set; }
}

public class CreateCategoryCommandHandler(
    IFileService fileService,
    IGenericRepository<Category> repository
    ) : IRequestHandler<CreateCategoryCommand>
{
    public async Task Handle(CreateCategoryCommand cmd, CancellationToken cancellationToken)
    {
        var imageUrl = cmd.Image is null 
            ? null 
            : await fileService.SaveImage(cmd.Image);

        await repository.InsertAsync(new Category 
        { 
            Name = cmd.Name,
            Description = cmd.Description,
            ImageUrl = imageUrl
        });
    }
}

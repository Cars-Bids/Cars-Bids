using MediatR;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;

namespace CarsAndBids.Core.CQRS.Categories;

public class DeleteCategoryByIdCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteCategoryByIdHandler(
    IFileService fileService,
    IGenericRepository<Category> repository
    ) : IRequestHandler<DeleteCategoryByIdCommand>
{
    public async Task Handle(DeleteCategoryByIdCommand cmd, CancellationToken cancellationToken)
    {
        var category = await repository.GetByIdAsync(cmd.Id);

        if (category?.ImageUrl is not null)
        {
            fileService.DeleteImage(category.ImageUrl);
        }

        await repository.DeleteAsync(cmd.Id);
    }
}

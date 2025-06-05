using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.Categories;

public class GetCategoryByIdQuery : IRequest<CategoryDto?>
{
    public int Id { get; set; }
}

public class GetCategoryByIdHandler(
    IMapper mapper,
    IGenericRepository<Category> repository
    ) : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
{
    public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await repository.GetByIdAsync(request.Id);

        return category is null
            ? null
            : mapper.Map<CategoryDto>(category);
    }
}

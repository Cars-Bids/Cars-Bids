using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.Categories;

public class GetAllCategoriesQuery : IRequest<List<CategoryDto>>
{    
}

public class GetAllCategoriesHandler(
    IMapper mapper,
    IGenericRepository<Category> repository
    ) : IRequestHandler<GetAllCategoriesQuery, List<CategoryDto>>
{
    public async Task<List<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await repository.GetAsync();

        return mapper.Map<List<CategoryDto>>(categories);
    }
}

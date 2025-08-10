using AutoMapper;
using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Models;
public class GetAllModelsQuery : IRequest<List<ModelDto>>
{
}

public class GetAllModelsHandler(
    IMapper mapper,
    IGenericRepository<Model> repository
    ) : IRequestHandler<GetAllModelsQuery, List<ModelDto>>
{
    public async Task<List<ModelDto>> Handle(GetAllModelsQuery request, CancellationToken cancellationToken)
    {
        var model = await repository.GetAsync();

        return mapper.Map<List<ModelDto>>(model);
    }
}

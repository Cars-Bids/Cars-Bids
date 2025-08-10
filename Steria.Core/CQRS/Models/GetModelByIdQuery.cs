using AutoMapper;
using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Models;

public class GetModelByIdQuery : IRequest<ModelDto?>
{
    public int Id { get; set; }
}

public class GetModelByIdHandler(
    IMapper mapper,
    IGenericRepository<Model> repository
    ) : IRequestHandler<GetModelByIdQuery, ModelDto?>
{
    public async Task<ModelDto?> Handle(GetModelByIdQuery request, CancellationToken cancellationToken)
    {
        var model = await repository.GetByIdAsync(request.Id);

        return model is null
            ? null
            : mapper.Map<ModelDto>(model);
    }
}

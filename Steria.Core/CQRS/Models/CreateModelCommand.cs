using AutoMapper;
using MediatR;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Models;

public class CreateModelCommand : IRequest
{
    public int MakeId { get; set; }
    public string? Name { get; set; }
}

public class CreateModelsCommandHandler(
    IGenericRepository<Model> repository,
    IMapper mapper
    ) : IRequestHandler<CreateModelCommand>
{
    public async Task Handle(CreateModelCommand cmd, CancellationToken cancellationToken)
    {
        var model = mapper.Map<Model>(cmd);
        await repository.InsertAsync(model);
    }
}
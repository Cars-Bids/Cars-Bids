using AutoMapper;
using Steria.Core.DTOs;
using MediatR;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Models;
public class UpdateModelCommand : IRequest
{
    public int Id { get; set; }
    public int MakeId { get; set; }
    public string? Name { get; set; }
}

public class UpdateModelCommandHandler(
    IGenericRepository<Model> repository,
    IMapper mapper
    ) : IRequestHandler<UpdateModelCommand>
{
    public async Task Handle(UpdateModelCommand cmd, CancellationToken cancellationToken)
    {
        var existingModel = await repository.GetByIdAsync(cmd.Id);

        mapper.Map(cmd, existingModel);

        await repository.UpdateAsync(existingModel!);

        return;
    }
}
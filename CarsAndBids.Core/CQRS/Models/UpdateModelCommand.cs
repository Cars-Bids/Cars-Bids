using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.Models;
public class UpdateModelCommand : IRequest<ModelDto>
{
    public required ModelDto Model { get; set; }
}

public class UpdateModelCommandHandler(
    IGenericRepository<Model> repository,
    IMapper mapper
    ) : IRequestHandler<UpdateModelCommand, ModelDto>
{
    public async Task<ModelDto> Handle(UpdateModelCommand cmd, CancellationToken cancellationToken)
    {
        var existingModel = await repository.GetByIdAsync(cmd.Model.Id);

        mapper.Map(cmd.Model, existingModel);

        await repository.UpdateAsync(existingModel!);

        return mapper.Map<ModelDto>(existingModel);
    }
}
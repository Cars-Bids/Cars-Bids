using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Exceptions;
using CarsAndBids.Core.Interfaces;
using MediatR;
using System.Net;

namespace CarsAndBids.Core.CQRS.Models;

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
        var model = await repository.GetByIdAsync(request.Id)
            ?? throw new HttpException("model not found", HttpStatusCode.NotFound);

        return mapper.Map<ModelDto>(model);
    }
}

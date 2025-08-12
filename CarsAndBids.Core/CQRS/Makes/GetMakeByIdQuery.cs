using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Exceptions;
using CarsAndBids.Core.Interfaces;
using MediatR;
using System.Net;

namespace CarsAndBids.Core.CQRS.Makes;

public class GetMakeByIdQuery : IRequest<MakeDto?>
{
    public int Id { get; set; }
}

public class GetMakeByIdHandler(
    IMapper mapper,
    IGenericRepository<Make> repository
    ) : IRequestHandler<GetMakeByIdQuery, MakeDto?>
{
    public async Task<MakeDto?> Handle(GetMakeByIdQuery request, CancellationToken cancellationToken)
    {
        var make = await repository.GetByIdAsync(request.Id)
            ?? throw new HttpException("make not found", HttpStatusCode.NotFound);

        return mapper.Map<MakeDto>(make);
    }
}

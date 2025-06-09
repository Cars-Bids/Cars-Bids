using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using MediatR;

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
        var make = await repository.GetByIdAsync(request.Id);

        return make is null
            ? null
            : mapper.Map<MakeDto>(make);
    }
}

using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Exceptions;
using CarsAndBids.Core.Interfaces;
using MediatR;
using System.Net;

namespace CarsAndBids.Core.CQRS.BodyStyles;

public class GetBodyStyleByIdQuery : IRequest<BodyStyleDto?>
{
    public int Id { get; set; }
}

public class GetBodyStyleByIdHandler(
    IMapper mapper,
    IGenericRepository<BodyStyle> repository
    ) : IRequestHandler<GetBodyStyleByIdQuery, BodyStyleDto?>
{
    public async Task<BodyStyleDto?> Handle(GetBodyStyleByIdQuery request, CancellationToken cancellationToken)
    {
        var bodyStyle = await repository.GetByIdAsync(request.Id)
            ?? throw new HttpException($"body style by id {request.Id} not found", HttpStatusCode.NotFound);

        return mapper.Map<BodyStyleDto>(bodyStyle);
    }
}

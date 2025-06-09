using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using MediatR;

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
        var bodyStyle = await repository.GetByIdAsync(request.Id);

        return bodyStyle is null
            ? null
            : mapper.Map<BodyStyleDto>(bodyStyle);
    }
}

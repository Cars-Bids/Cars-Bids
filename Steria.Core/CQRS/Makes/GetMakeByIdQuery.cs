using System.Net;
using AutoMapper;
using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Exceptions;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Makes;

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

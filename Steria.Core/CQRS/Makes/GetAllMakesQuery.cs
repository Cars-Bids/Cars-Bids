using AutoMapper;
using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Makes;

public class GetAllMakesQuery : IRequest<List<MakeDto>> {}

public class GetAllMakesHandler(
    IMapper mapper,
    IGenericRepository<Make> repository
    ) : IRequestHandler<GetAllMakesQuery, List<MakeDto>>
{
    public async Task<List<MakeDto>> Handle(GetAllMakesQuery request, CancellationToken cancellationToken)
    {
        var make = await repository.GetAsync();

        return mapper.Map<List<MakeDto>>(make);
    }
}

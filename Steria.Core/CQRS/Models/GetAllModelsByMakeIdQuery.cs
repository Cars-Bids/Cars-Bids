using AutoMapper;
using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.ModelSpec;

namespace Steria.Core.CQRS.Models;

public class GetAllModelsByMakeIdQuery : IRequest<List<ModelDto>>
{
    public int MakeId { get; set; }
}

public class GetAllModelsByMakeIdQueryHandler(IMapper mapper,
                                              IGenericRepository<Model> modelRepository) : IRequestHandler<GetAllModelsByMakeIdQuery, List<ModelDto>>
{
    public async Task<List<ModelDto>> Handle(GetAllModelsByMakeIdQuery request, CancellationToken cancellationToken)
    {
        var models = await modelRepository.GetListBySpec(new ModelsByMakeIdSpec(request.MakeId), cancellationToken);
        return mapper.Map<List<ModelDto>>(models);
    }
}
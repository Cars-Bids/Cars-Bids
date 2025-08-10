using System.Security.Claims;
using AutoMapper;
using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Profile;

public class GetProfileByIdQuery : IRequest<ProfileDto?>
{
    public int UserId { get; set; }
}

public class GetProfileByIdHandler(
    IMapper mapper,
    IGenericRepository<User> repository
    ) : IRequestHandler<GetProfileByIdQuery, ProfileDto?>
{
    public async Task<ProfileDto?> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var profile = await repository.GetByIdAsync(request.UserId);

        return profile is null
            ? null
            : mapper.Map<ProfileDto>(profile);
    }
}
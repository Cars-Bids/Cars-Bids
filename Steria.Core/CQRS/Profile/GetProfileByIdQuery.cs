using AutoMapper;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Exceptions;
using Steria.Core.Interfaces;
using Steria.Core.Specification.ProfileSpec;
using MediatR;
using System.Net;

namespace Steria.Core.CQRS.Profile;

public class GetProfileByIdQuery : IRequest<ProfileDto?>
{
    public int UserId { get; set; }
}

public class GetProfileByIdHandler(
    IGenericRepository<User> repository,
    IMapper mapper
    ) : IRequestHandler<GetProfileByIdQuery, ProfileDto?>
{
    public async Task<ProfileDto?> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new UserProfileSpec(request.UserId);
        var profile = await repository.GetItemBySpec(spec, cancellationToken);

        if (profile == null)
        {
            throw new HttpException("Profile not found", HttpStatusCode.NotFound);
        }

        return profile;
    }
}
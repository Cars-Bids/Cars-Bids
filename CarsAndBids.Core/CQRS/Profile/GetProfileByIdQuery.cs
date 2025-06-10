using System.Security.Claims;
using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CarsAndBids.Core.CQRS.Profile;

public class GetProfileByIdQuery : IRequest<ProfileDto?>
{
}

public class GetProfileByIdHandler(
    IMapper mapper,
    IGenericRepository<User> repository,
    IHttpContextAccessor httpContextAccessor
    ) : IRequestHandler<GetProfileByIdQuery, ProfileDto?>
{
    public async Task<ProfileDto?> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
    {
        int userId = int.Parse(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var profile = await repository.GetByIdAsync(userId);

        return profile is null
            ? null
            : mapper.Map<ProfileDto>(profile);
    }
}

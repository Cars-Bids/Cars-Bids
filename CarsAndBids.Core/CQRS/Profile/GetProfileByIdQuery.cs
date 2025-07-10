using System.Security.Claims;
using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.Profile;

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
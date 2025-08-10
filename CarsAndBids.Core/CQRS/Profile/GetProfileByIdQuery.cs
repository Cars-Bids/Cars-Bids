using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Exceptions;
using CarsAndBids.Core.Interfaces;
using MediatR;
using System.Net;

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
        var profile = await repository.GetByIdAsync(request.UserId)
            ?? throw new HttpException("profile not found", HttpStatusCode.NotFound);

        return mapper.Map<ProfileDto>(profile);
    }
}
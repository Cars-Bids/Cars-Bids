using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CarsAndBids.Core.CQRS.Profile;

public class UpdateProfileCommand : IRequest<ProfileDto>
{
    public required ProfileDto Profile { get; set; }
}

public class UpdateProfileCommandHandler(
    IGenericRepository<User> repository,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper
    ) : IRequestHandler<UpdateProfileCommand, ProfileDto>
{
    public async Task<ProfileDto> Handle(UpdateProfileCommand cmd, CancellationToken cancellationToken)
    {
        int userId = int.Parse(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var existingUser = await repository.GetByIdAsync(cmd.Profile.Id);

        if (existingUser?.Id != userId)
        {
            throw new UnauthorizedAccessException("This is not your profile.");
        }

        mapper.Map(cmd.Profile, existingUser);

        await repository.UpdateAsync(existingUser);

        return mapper.Map<ProfileDto>(existingUser);
    }
}

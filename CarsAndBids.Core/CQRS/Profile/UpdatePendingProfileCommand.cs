using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CarsAndBids.Core.CQRS.Profile;

public class UpdateProfileCommand : IRequest
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ProfilePictureUrl { get; set; }
}

public class UpdateProfileCommandHandler(
    IGenericRepository<User> repository,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper
    ) : IRequestHandler<UpdateProfileCommand>
{
    public async Task Handle(UpdateProfileCommand cmd, CancellationToken cancellationToken)
    {
        int userId = int.Parse(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var existingUser = await repository.GetByIdAsync(userId);

        if (existingUser?.Id != userId)
        {
            throw new UnauthorizedAccessException("This is not your profile.");
        }

        mapper.Map(cmd, existingUser);

        await repository.UpdateAsync(existingUser);

        return;
    }
}

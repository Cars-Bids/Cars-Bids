using AutoMapper;
using Steria.Core.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Profile;

public class UpdateProfileCommand : IRequest
{
    public int UserId { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ProfilePictureUrl { get; set; }
}

public class UpdateProfileCommandHandler(
    IGenericRepository<User> repository,
    IMapper mapper
) : IRequestHandler<UpdateProfileCommand>
{
    public async Task Handle(UpdateProfileCommand cmd, CancellationToken cancellationToken)
    {
        var existingUser = await repository.GetByIdAsync(cmd.UserId);

        if (existingUser == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        mapper.Map(cmd, existingUser);

        await repository.UpdateAsync(existingUser);
    }
}


using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Steria.Core.Constants;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Resources;


namespace Steria.Core.CQRS.Account;

public class RegisterCommand : IRequest //TODO: Add localization
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public IFormFile? Image { get; set; }
}

public class RegisterCommandHandler(
    IFileService fileService,
    UserManager<User> userManager,
    IMapper mapper
    ) : IRequestHandler<RegisterCommand>
{
    public async Task Handle(RegisterCommand cmd, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(cmd.Email!);
        if (user is not null) 
            throw new Exception(string.Format(Resource.UserAlreadyExists, cmd.Email));

        var newUser = mapper.Map<User>(cmd);

        var result = await userManager.CreateAsync(newUser, cmd.Password!);
            
        if (result.Succeeded)
            await userManager.AddToRoleAsync(newUser, Roles.User);
        else
            throw new Exception($"Error creating user with {cmd.Email}");
    }
}
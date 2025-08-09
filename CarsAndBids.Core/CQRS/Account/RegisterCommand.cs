using AutoMapper;
using CarsAndBids.Core.Constants;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using CarsAndBids.Core.Resources;


namespace CarsAndBids.Core.CQRS.Account;

public class RegisterCommand : IRequest
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public IFormFile? Image { get; set; }
}

public class RegisterCommandHandler(IFileService fileService,
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
            
            newUser.ProfilePictureUrl = cmd.Image is null ? null : 
                await fileService.UploadImageAsync(cmd.Image);
            
            var result = await userManager.CreateAsync(newUser, cmd.Password!);
            
            if (result.Succeeded)
                await userManager.AddToRoleAsync(newUser, Roles.User);
            else
                throw new Exception($"Error creating user with {cmd.Email}");
    }
}
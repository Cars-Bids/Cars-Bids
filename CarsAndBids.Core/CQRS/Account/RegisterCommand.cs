using AutoMapper;
using CarsAndBids.Core.Constants;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;


namespace CarsAndBids.Core.CQRS.Account;

public class RegisterCommand : IRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
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
                throw new Exception($"User with {cmd.Email} already exist!");

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
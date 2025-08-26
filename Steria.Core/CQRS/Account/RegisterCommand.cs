using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Steria.Core.Constants;
using Steria.Core.Entities;
using Steria.Core.Exceptions;
using Steria.Core.Interfaces;
using Steria.Core.Resources;


namespace Steria.Core.CQRS.Account;

public class RegisterCommand : IRequest
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class RegisterCommandHandler(
    IFileService fileService,
    UserManager<User> userManager,
    IMapper mapper,
    IGenericRepository<NotificationType> notifTypeRepository,
    IGenericRepository<UserNotificationSetting> settingRepository) : IRequestHandler<RegisterCommand>
{
    public async Task Handle(RegisterCommand cmd, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(cmd.Email);
        if (user is not null) 
            throw new HttpException(string.Format(Resource.UserAlreadyExists, cmd.Email), HttpStatusCode.Conflict);

        var usernameTaken = await userManager.FindByNameAsync(cmd.Username);
        if (usernameTaken is not null)
            throw new HttpException(string.Format(Resource.UsernameAlreadyExists, cmd.Username), HttpStatusCode.Conflict);
        
        var newUser = mapper.Map<User>(cmd);

        var result = await userManager.CreateAsync(newUser, cmd.Password!);
            
        if (result.Succeeded)
            await userManager.AddToRoleAsync(newUser, Roles.User);
        else
            throw new Exception($"Error creating user with {cmd.Email}");

        var allNotifTypes = await notifTypeRepository.GetAsync(cancellationToken: cancellationToken);
        var defaultSettings = allNotifTypes.Select(nt => new UserNotificationSetting
        {
            UserId = newUser.Id,
            NotificationTypeId = nt.Id,
            SendEmail = nt.DefaultSendEmail,
            SendInSite = nt.DefaultSendSite
        }).ToList();

        await settingRepository.InsertRangeAsync(defaultSettings);
    }
}
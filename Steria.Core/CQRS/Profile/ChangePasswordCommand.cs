using Steria.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Steria.Core.CQRS.Profile;

public class ChangePasswordCommand : IRequest<bool>
{
    public int UserId { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}

public class ChangePasswordCommandHandler(
    UserManager<User> userManager
) : IRequestHandler<ChangePasswordCommand, bool>
{
    public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            return false;

        var result = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        return result.Succeeded;
    }
}
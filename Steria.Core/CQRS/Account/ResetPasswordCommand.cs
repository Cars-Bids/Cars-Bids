using MediatR;
using Microsoft.AspNetCore.Identity;
using Steria.Core.Entities;

namespace Steria.Core.CQRS.Account;

public class ResetPasswordCommand : IRequest<bool>
{
    public string Email { get; set; }
    public string Token { get; set; }
    public string NewPassword { get; set; }
}

public class ResetPasswordCommandHandler(
        UserManager<User> userManager
    ) : IRequestHandler<ResetPasswordCommand, bool>
{
    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return false;

        var result = await userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        return result.Succeeded;
    }
}
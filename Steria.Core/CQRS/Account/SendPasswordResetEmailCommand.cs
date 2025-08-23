using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Steria.Core.Entities;
using Steria.Core.Models;
using Steria.Core.Interfaces;
using System.Reflection;
using Steria.Core.DTOs;
using System.Web;

namespace Steria.Core.CQRS.Account;

public class SendPasswordResetEmailCommand : IRequest<bool>
{
    public string MailTo { get; set; }
}

public class SendPasswordResetEmailCommandHandler(
        UserManager<User> userManager,
        IOptions<EmailSettings> emailSettings,
        IEmailQueue emailQueue) : IRequestHandler<SendPasswordResetEmailCommand, bool>
{
    public async Task<bool> Handle(SendPasswordResetEmailCommand request, CancellationToken cancellationToken)
    {
        if (emailSettings == null)
            throw new ArgumentNullException(nameof(emailSettings));

        var settings = emailSettings.Value;
        if (settings == null)
            throw new ArgumentNullException(nameof(settings));

        var user = await userManager.FindByEmailAsync(request.MailTo);
        if (user == null)
            return false;

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "Steria.Core.Templates.PasswordResetTemplate.html";
        string htmlBody;

        using (var stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream == null)
                throw new FileNotFoundException("Template file not found.", resourceName);

            using var reader = new StreamReader(stream);
            htmlBody = await reader.ReadToEndAsync();
        }

        htmlBody = htmlBody.Replace("{{ResetToken}}", HttpUtility.UrlEncode(token))
                           .Replace("{{Email}}", HttpUtility.UrlEncode(request.MailTo));

        var emailTask = new EmailTask
        {
            MailTo = request.MailTo,
            Subject = "Password Reset",
            HtmlBody = htmlBody,
            FromName = "CarsAndBids",
            FromEmail = settings.Username
        };

        emailQueue.Enqueue(emailTask);

        return true;
    }
}
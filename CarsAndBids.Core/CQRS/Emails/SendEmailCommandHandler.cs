using MediatR;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using CarsAndBids.Core.DTOs;
using System.Reflection;
using CarsAndBids.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace CarsAndBids.Core.CQRS.Emails;

public class SendPasswordResetEmailCommand : IRequest<bool>
{
    public string To { get; set; }
}

public class SendPasswordResetEmailCommandHandler(
    UserManager<User> userManager,
    IOptions<EmailSettings> emailSettings
) : IRequestHandler<SendPasswordResetEmailCommand, bool>
{
    public async Task<bool> Handle(SendPasswordResetEmailCommand request, CancellationToken cancellationToken)
    {
        if (emailSettings == null)
            throw new ArgumentNullException(nameof(emailSettings));

        var settings = emailSettings.Value;
        if (settings == null)
            throw new ArgumentNullException(nameof(settings));

        var user = await userManager.FindByEmailAsync(request.To);
        if (user == null)
            return false;

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "CarsAndBids.Core.Templates.PasswordResetTemplate.html";
        string htmlBody;

        using (var stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream == null)
                throw new FileNotFoundException("Template file not found.", resourceName);

            using var reader = new StreamReader(stream);
            htmlBody = await reader.ReadToEndAsync();
        }

        htmlBody = htmlBody.Replace("{{ResetToken}}", token);
        htmlBody = htmlBody.Replace("{{UserEmail}}", request.MailTo);

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("CarsAndBids", settings.Username ?? throw new ArgumentNullException(nameof(settings.Username))));
        message.To.Add(new MailboxAddress(request.To ?? throw new ArgumentNullException(nameof(request.To)), request.To));
        message.Subject = "Password Reset";
        message.Body = new TextPart("html") { Text = htmlBody };
    
        using var client = new SmtpClient();
        await client.ConnectAsync(settings.SmtpServer ?? throw new ArgumentNullException(nameof(settings.SmtpServer)), settings.Port, SecureSocketOptions.StartTls, cancellationToken);
        await client.AuthenticateAsync(settings.Username ?? throw new ArgumentNullException(nameof(settings.Username)), settings.Password ?? throw new ArgumentNullException(nameof(settings.Password)), cancellationToken);
        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);

        return true;
    }
}

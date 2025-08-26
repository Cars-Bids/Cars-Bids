using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using Steria.Core.DTOs;
using Steria.Core.Interfaces;

namespace Steria.Data.Services;

public class EmailBackgroundService(
        IEmailQueue emailQueue,
        IOptions<EmailSettings> emailSettings) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var settings = emailSettings.Value;

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var emailTask = await emailQueue.DequeueAsync(stoppingToken);

                using var client = new SmtpClient();
                await client.ConnectAsync(settings.SmtpServer, settings.Port, SecureSocketOptions.StartTls, stoppingToken);
                await client.AuthenticateAsync(settings.Username, settings.Password, stoppingToken);

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(emailTask.FromName, emailTask.FromEmail));
                message.To.Add(new MailboxAddress(emailTask.MailTo, emailTask.MailTo));
                message.Subject = emailTask.Subject;
                message.Body = new TextPart("html") { Text = emailTask.HtmlBody };

                await client.SendAsync(message, stoppingToken);
                await client.DisconnectAsync(true, stoppingToken);

            }
            catch (Exception ex)
            {
            }
        }
    }
}
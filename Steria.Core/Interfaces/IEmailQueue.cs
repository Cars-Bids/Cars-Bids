using Steria.Core.DTOs;

namespace Steria.Core.Interfaces;

public interface IEmailQueue
{
    void Enqueue(EmailTask emailTask);
    Task<EmailTask> DequeueAsync(CancellationToken cancellationToken);
}
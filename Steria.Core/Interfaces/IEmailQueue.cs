using Steria.Core.Models;

namespace Steria.Core.Interfaces;

public interface IEmailQueue
{
    void Enqueue(EmailTask emailTask);
    Task<EmailTask> DequeueAsync(CancellationToken cancellationToken);
}
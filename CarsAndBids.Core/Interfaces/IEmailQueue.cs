using CarsAndBids.Core.Models;

namespace CarsAndBids.Core.Interfaces;

public interface IEmailQueue
{
    void Enqueue(EmailTask emailTask);
    Task<EmailTask> DequeueAsync(CancellationToken cancellationToken);
}
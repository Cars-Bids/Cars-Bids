using System.Collections.Concurrent;
using System.Threading.Channels;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Core.Models;

namespace CarsAndBids.Core.Services;

public class EmailQueue : IEmailQueue
{
    private readonly Channel<EmailTask> _queue;

    public EmailQueue()
    {
        _queue = Channel.CreateUnbounded<EmailTask>();
    }

    public void Enqueue(EmailTask emailTask)
    {
        _queue.Writer.TryWrite(emailTask);
    }

    public async Task<EmailTask> DequeueAsync(CancellationToken cancellationToken)
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }
}
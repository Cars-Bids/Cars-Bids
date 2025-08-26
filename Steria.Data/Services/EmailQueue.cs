using System.Threading.Channels;
using Steria.Core.DTOs;
using Steria.Core.Interfaces;

namespace Steria.Data.Services;

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
using MediatR;
using AutoMapper;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Auctions;

public class AddAnswerCommand : IRequest
{
    public int QuestionId { get; set; }
    public int UserId { get; set; }
    public string AnswerText { get; set; } = null!;
}

public class AddAnswerCommandHandler(
    IMapper mapper,
    IGenericRepository<Answer> answerRepo
    ) : IRequestHandler<AddAnswerCommand>
{
    public async Task Handle(AddAnswerCommand cmd, CancellationToken cancellationToken)
    {
        var answer = mapper.Map<Answer>(cmd);
        answer.CreatedAt = DateTime.UtcNow;
        await answerRepo.InsertAsync(answer);
    }
}

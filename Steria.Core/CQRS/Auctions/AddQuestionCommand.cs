using MediatR;
using AutoMapper;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Auctions;

public class AddQuestionCommand : IRequest
{
    public int AuctionId { get; set; }
    public int UserId { get; set; }
    public string QuestionText { get; set; } = null!;
}

public class AddQuestionCommandHandler(
    IMapper mapper,
    IGenericRepository<Question> questionRepo
    ) : IRequestHandler<AddQuestionCommand>
{
    public async Task Handle(AddQuestionCommand cmd, CancellationToken cancellationToken)
    {
        var question = mapper.Map<Question>(cmd);
        question.CreatedAt = DateTime.UtcNow;
        await questionRepo.InsertAsync(question);
    }
}

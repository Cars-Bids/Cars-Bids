using AutoMapper;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using MediatR;

namespace Steria.Core.CQRS.Comments;
public class CreateCommentCommand : IRequest
{
    public int AuctionId { get; set; }
    public int UserId { get; set; }
    public string Text { get; set; } = null!;
}

public class CreateCommentsCommandHandler(
    IGenericRepository<Comment> repository,
    IMapper mapper
    ) : IRequestHandler<CreateCommentCommand>
{
    public async Task Handle(CreateCommentCommand cmd, CancellationToken cancellationToken)
    {
        var comment = mapper.Map<Comment>(cmd);
        await repository.InsertAsync(comment);
    }
}
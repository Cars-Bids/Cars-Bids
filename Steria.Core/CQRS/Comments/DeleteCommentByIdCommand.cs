using Steria.Core.Entities;
using Steria.Core.Interfaces;
using MediatR;

namespace Steria.Core.CQRS.Comments;

public class DeleteCommentByIdCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteCommentByIdHandler(
    IGenericRepository<Comment> repository
    ) : IRequestHandler<DeleteCommentByIdCommand>
{
    public async Task Handle(DeleteCommentByIdCommand cmd, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(cmd.Id);
    }
}

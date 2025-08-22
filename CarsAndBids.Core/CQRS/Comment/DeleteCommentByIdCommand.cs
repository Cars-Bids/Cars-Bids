using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.Comments;

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

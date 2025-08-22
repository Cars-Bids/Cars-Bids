using AutoMapper;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.Comments;

public class UpdateCommentCommand : IRequest
{
    public int Id { get; set; }
    public int AuctionId { get; set; }
    public int UserId { get; set; }
    public string Text { get; set; } = null!;
}

public class UpdateCommentCommandHandler(
    IGenericRepository<Comment> repository,
    IMapper mapper
    ) : IRequestHandler<UpdateCommentCommand>
{
    public async Task Handle(UpdateCommentCommand cmd, CancellationToken cancellationToken)
    {
        var existingComment = await repository.GetByIdAsync(cmd.Id);

        mapper.Map(cmd, existingComment);

        await repository.UpdateAsync(existingComment!);

        return;
    }
}

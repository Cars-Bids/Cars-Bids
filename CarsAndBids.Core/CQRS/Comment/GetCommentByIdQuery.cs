using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Exceptions;
using CarsAndBids.Core.Interfaces;
using MediatR;
using System.Net;

namespace CarsAndBids.Core.CQRS.Comments;

public class GetCommentByIdQuery : IRequest<CommentDto?>
{
    public int Id { get; set; }
}

public class GetCommentByIdHandler(
    IMapper mapper,
    IGenericRepository<Comment> repository
    ) : IRequestHandler<GetCommentByIdQuery, CommentDto?>
{
    public async Task<CommentDto?> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
    {
        var Comment = await repository.GetByIdAsync(request.Id)
            ?? throw new HttpException("Comment not found", HttpStatusCode.NotFound);

        return mapper.Map<CommentDto>(Comment);
    }
}

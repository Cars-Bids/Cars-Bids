using System.Net;
using MediatR;
using Steria.Core.Entities;
using Steria.Core.Exceptions;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Wishlists;

public class DeleteSavedSearchCommand : IRequest
{
    public int Id { get; set; }
    public int UserId { get; set; } 
}

public class DeleteSavedSearchCommandHandler(
    IGenericRepository<SavedSearch> repository
) : IRequestHandler<DeleteSavedSearchCommand>
{
    public async Task Handle(DeleteSavedSearchCommand cmd, CancellationToken cancellationToken)
    {
        var savedSearch = await repository.GetByIdAsync(cmd.Id);
        if (savedSearch == null || savedSearch.UserId != cmd.UserId)
        {
            throw new HttpException("Saved search not found or access denied", HttpStatusCode.NotFound);
        }

        await repository.DeleteAsync(savedSearch);
        await repository.SaveAsync();
    }
}
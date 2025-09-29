using AutoMapper;
using MediatR;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Wishlists;

public class CreateSavedSearchCommand : IRequest
{
    public int UserId { get; set; }
    public int MakeId { get; set; }
    public int? ModelId { get; set; }
}

public class CreateSavedSearchCommandHandler(
    IGenericRepository<SavedSearch> repository,
    IMapper mapper
    ) : IRequestHandler<CreateSavedSearchCommand>
{
    public async Task Handle(CreateSavedSearchCommand cmd, CancellationToken cancellationToken)
    {
        var savedSearch = mapper.Map<SavedSearch>(cmd);
        await repository.InsertAsync(savedSearch);
        await repository.SaveAsync();
    }
}
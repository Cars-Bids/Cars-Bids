using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using MediatR;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Wishlists;

public class CheckSavedSearchExistsQuery : IRequest<bool>
{
    public int UserId { get; set; }
    public int MakeId { get; set; }
    public int? ModelId { get; set; }
}

public class CheckSavedSearchExistsQueryHandler(
    IGenericRepository<SavedSearch> repository
) : IRequestHandler<CheckSavedSearchExistsQuery, bool>
{
    public async Task<bool> Handle(CheckSavedSearchExistsQuery request, CancellationToken cancellationToken)
    {
        var spec = new Specification<SavedSearch>();
        spec.Query
            .Where(s => s.UserId == request.UserId && s.MakeId == request.MakeId && s.ModelId == request.ModelId)
            .AsNoTracking();

        var count = await repository.CountAsync(spec, cancellationToken);
        return count > 0;
    }
}
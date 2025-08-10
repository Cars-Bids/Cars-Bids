using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Wishlists;

public class DeleteWishlistByIdCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteWishlistByIdHandler(
    IGenericRepository<Wishlist> repository
    ) : IRequestHandler<DeleteWishlistByIdCommand>
{
    public async Task Handle(DeleteWishlistByIdCommand cmd, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(cmd.Id);
    }
}

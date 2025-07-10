using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using MediatR;

namespace CarsAndBids.Core.Commands.BodyStyles;

public class DeleteBodyStyleByIdCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteBodyStyleByIdHandler(
    IGenericRepository<BodyStyle> repository
    ) : IRequestHandler<DeleteBodyStyleByIdCommand>
{
    public async Task Handle(DeleteBodyStyleByIdCommand cmd, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(cmd.Id);
    }
}

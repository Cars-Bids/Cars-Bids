using Ardalis.Specification;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Specification.СommonSpec;


namespace CarsAndBids.Data.Persistence.Repositories.Specification.BodyStyleSpec;

public class PagedBodyStylesSpec : PagedSpec<BodyStyle>
{
    public PagedBodyStylesSpec(int pageNumber, int pageSize)
        : base(pageNumber, pageSize)
    {
        Query.OrderBy(bs => bs.StyleName); // sort (unbonded)

    }
}

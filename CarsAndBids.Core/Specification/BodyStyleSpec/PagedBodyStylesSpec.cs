using Ardalis.Specification;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Specification.CommonSpec;


namespace CarsAndBids.Core.Specification.BodyStyleSpec;

public class PagedBodyStylesSpec : PagedSpec<BodyStyle>
{
    public PagedBodyStylesSpec(int pageNumber, int pageSize)
        : base(pageNumber, pageSize)
    {
        Query.OrderBy(bs => bs.StyleName);

    }
}

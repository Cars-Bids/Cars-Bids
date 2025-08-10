using Ardalis.Specification;
using Steria.Core.Entities;
using Steria.Core.Specification.СommonSpec;


namespace Steria.Core.Specification.BodyStyleSpec;

public class PagedBodyStylesSpec : PagedSpec<BodyStyle>
{
    public PagedBodyStylesSpec(int pageNumber, int pageSize)
        : base(pageNumber, pageSize)
    {
        Query.OrderBy(bs => bs.StyleName); // sort (unbonded)

    }
}

using Ardalis.Specification;
using Steria.Core.Entities;

namespace Steria.Core.Specification.ModelSpec;

public class ModelWithMakeSpec : Specification<Model, Model>
{
    public ModelWithMakeSpec()
    {
        Query.Include(m => m.Make)
            .Select(m => m);
    }
}
using Ardalis.Specification;
using Steria.Core.Entities;

namespace Steria.Core.Specification.ModelSpec;

public class ModelsByMakeIdSpec : Specification<Model, Model>
{
    public ModelsByMakeIdSpec(int makeId)
    {
        Query.Where(x => x.MakeId == makeId)
            .Select(x => x);
    }
}
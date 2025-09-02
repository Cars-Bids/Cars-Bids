using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.СommonSpec;

namespace Steria.Data.Persistence.Seed;

public class MakeSeeder(IGenericRepository<Make> repository)
{
    public async Task SeedAsync()
    {
        var existing = await repository.GetItemBySpec(new FirstRecordSpec<Make>());

        if (existing is null)
        {
            var makeNames = new string[]
            {
                "Toyota", "Ford", "Chevrolet", "Honda", "Nissan", "Hyundai", "Kia", "Volkswagen", "BMW",
                "Mercedes-Benz",
                "Subaru", "Mazda", "Tesla", "Audi", "Jeep", "Ram", "GMC", "Lexus", "Porsche", "Volvo"
            };

            var makes = makeNames.Select(name => new Make { Name = name }).ToList();

            await repository.InsertRangeAsync(makes);
        }
    }
}
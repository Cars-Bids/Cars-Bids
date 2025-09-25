namespace Steria.Core.Entities;

public class Model
{
    public int Id { get; set; }
    public int MakeId { get; set; }
    public string Name { get; set; } = null!;

    public Make Make { get; set; } = null!;
    public ICollection<Car>? Cars { get; set; }
    public ICollection<SavedSearch> SavedSearches { get; set; }
}
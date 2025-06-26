namespace CarsAndBids.Data.Entities;

public class BodyStyle
{
    public int Id { get; set; }
    public string? StyleName { get; set; }

    public ICollection<Car>? Cars { get; set; }
}
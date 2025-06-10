namespace CarsAndBids.Data.Entities;

public class Model
{
    public int Id { get; set; }
    public int MakeId { get; set; }
    public string? Name { get; set; }

    public Make? Make { get; set; }
    public ICollection<Car>? Cars { get; set; }
    public ICollection<PendingCar>? PendingCars { get; set; }
}
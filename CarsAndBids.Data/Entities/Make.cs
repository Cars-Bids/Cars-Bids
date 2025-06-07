namespace CarsAndBids.Data.Entities;

public class Make
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Model> Models { get; set; }
}
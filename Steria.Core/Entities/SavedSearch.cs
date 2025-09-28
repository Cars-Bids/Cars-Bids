namespace Steria.Core.Entities;

public class SavedSearch
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int? ModelId { get; set; }
    public int MakeId { get; set; }

    public User User { get; set; }
    public Model? Model { get; set; }
    public Make Make { get; set; }
}
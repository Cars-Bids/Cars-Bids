namespace CarsAndBids.Data.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string? Token { get; set; }
    public DateTime ExpiresOnUtc { get; set; }
    public int UserId { get; set; }

    public User? User { get; set; }
}
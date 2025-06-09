namespace CarsAndBids.Data.Entities;

public class Answer
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public int UserId { get; set; }
    public string? AnswerText { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Question? Question { get; set; }
    public User? User { get; set; }
}
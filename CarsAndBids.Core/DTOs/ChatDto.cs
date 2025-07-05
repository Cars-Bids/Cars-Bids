using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Enums;

namespace CarsAndBids.Core.DTOs;

public class ChatDto
{
    public int Id { get; set; }
    public int? CarId { get; set; }
    public ICollection<int>? ParticipantsId { get; set; }
}
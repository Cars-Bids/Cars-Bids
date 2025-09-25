using Steria.Core.Enums;

namespace Steria.Core.DTOs;

public class CarImageDto
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public string? ImageUrl { get; set; }
    public ImageCategory ImageCategory { get; set; }
    public int OrderNumber { get; set; }
}
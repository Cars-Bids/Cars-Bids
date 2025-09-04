using Steria.Core.Enums;

namespace Steria.Core.DTOs;

public class CarPreviewDto
{
    public string Description { get; set; }
    public string Location { get; set; }
    public TransmissionType TransmissionType { get; set; }
    public int Mileage { get; set; }
    public int Year { get; set; }
    public string Model { get; set; }
    public string Make { get; set; }
    public string MainImageUrl { get; set; }
}
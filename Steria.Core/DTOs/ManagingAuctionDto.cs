namespace Steria.Core.DTOs;

public class ManagingAuctionPageDto
{
    public int CarId { get; set; }
    public int CarModelId { get; set; }
    public int CarBrandId { get; set; }
    public int? CarMileage { get; set; }
    public int? CarYear { get; set; }
    public string? CarVin { get; set; }
    public string? CarLocation { get; set; }
    public string? CarExteriorColor { get; set; }
    public string? CarInteriorColor { get; set; }
    public string? CarEngine { get; set; }
    public int? CarDriveTrainId { get; set; }
    public int? CarTransmissionId { get; set; }
    public int? CarBodyStyleId { get; set; }
    public int? CarSpeeds { get; set; }
    public string? CarHighlights { get; set; }
    public string? CarServiceHistory { get; set; }
    public string? CarEquipment { get; set; }
    public string? CarFlaws { get; set; }
    public string? CarModifications { get; set; }
    public string? CarOtherItems { get; set; }
    public string? CarOwnershipHistory { get; set; }
    public string? CarSellerNotes { get; set; }
    public string? CarVideoLinks { get; set; }
    public string? CarAbout { get; set; }

    //photos
    public string? CarMainPhotoUrl { get; set; }
    public List<CarImageDto> CarInteriorPhotoUrls { get; set; }
    public List<CarImageDto> CarExteriorPhotoUrls { get; set; }
    public List<CarImageDto> CarOtherPhotoUrls { get; set; }
    
    //auction part
    public int AuctionId { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public bool IsInspected { get; set; } = false;
    public int? StartPrice { get; set; }
    public string SellerUsername { get; set; }
}
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Cars;

public class CreateCarCommand : IRequest
{
    public int OwnerId { get; set; }
    public int Year { get; set; }
    public string? Vin { get; set; }
    public string? Description { get; set; }
    public string? ExteriorColor { get; set; }
    public string? InteriorColor { get; set; }
    public int Mileage { get; set; }
    public string? Location { get; set; }
    public DrivetrainType Drivetrain { get; set; }
    public string? Engine { get; set; }
    public TransmissionType TransmissionType { get; set; }
    public int Speeds { get; set; }
    public int? AssingId { get; set; }
    public int BodyStyleId { get; set; }
    public int ModelId { get; set; }
    public IFormFile? MainImage { get; set; }
    public List<IFormFile>? ExteriorImages { get; set; }
    public List<IFormFile>? InteriorImages { get; set; }
    public List<IFormFile>? OtherImages { get; set; }
}

public class CreateCarCommandHandler(
    IGenericRepository<Car> carRepository,
    IGenericRepository<CarImage> carImageRepository,
    IMapper mapper,
    IFileService fileService
    ) : IRequestHandler<CreateCarCommand>
{
    
    public async Task Handle(CreateCarCommand cmd, CancellationToken cancellationToken)
    {

        var car = mapper.Map<Car>(cmd);
        car.OwnerId = cmd.OwnerId;
        car.Status = CarStatus.inPending;

        await carRepository.InsertAsync(car);

        int orderNumber = 1;

        if (cmd.MainImage != null)
        {
            var ImageUrl = await fileService.UploadImageAsync(cmd.MainImage);
            await carImageRepository.InsertAsync(new CarImage
            {
                CarId = car.Id,
                ImageUrl = ImageUrl,
                ImageCategory = ImageCategory.Main,
                OrderNumber = orderNumber++,
                UploadedAt = DateTime.UtcNow
            });

        }

        if (cmd.ExteriorImages != null)
        {
            var ImageUrls = await fileService.UploadImagesAsync(cmd.ExteriorImages);
            foreach (var url in ImageUrls)
            {
                await carImageRepository.InsertAsync(new CarImage
                {
                    CarId = car.Id,
                    ImageUrl = url,
                    ImageCategory = ImageCategory.Exterior,
                    OrderNumber = orderNumber++,
                    UploadedAt = DateTime.UtcNow
                });
            }
        }

        if (cmd.InteriorImages != null)
        {
            var ImageUrls = await fileService.UploadImagesAsync(cmd.InteriorImages);
            foreach (var url in ImageUrls)
            {
                await carImageRepository.InsertAsync(new CarImage
                {
                    CarId = car.Id,
                    ImageUrl = url,
                    ImageCategory = ImageCategory.Interior,
                    OrderNumber = orderNumber++,
                    UploadedAt = DateTime.UtcNow
                });
            }
        }

        if (cmd.OtherImages != null)
        {
            var ImageUrls = await fileService.UploadImagesAsync(cmd.OtherImages);
            foreach (var url in ImageUrls)
            {
                await carImageRepository.InsertAsync(new CarImage
                {
                    CarId = car.Id,
                    ImageUrl = url,
                    ImageCategory = ImageCategory.Other,
                    OrderNumber = orderNumber++,
                    UploadedAt = DateTime.UtcNow
                });
            }
        }

        car.CreatedAt = DateTime.UtcNow;

    }
}
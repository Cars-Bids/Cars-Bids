using AutoMapper;
using CarsAndBids.Core.Exceptions;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Enums;
using CarsAndBids.Data.Persistence.Repositories.Specification.CarSpec;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace CarsAndBids.Core.Commands.Cars;

public class UpdateCarCommand : IRequest
{
    public int Id { get; set; }
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
    public CarStatus Status { get; set; }
    public int OwnerId { get; set; }
    public int AssingId { get; set; }
    public int BodyStyleId { get; set; }
    public int ModelId { get; set; }
    public List<ImageUpdateRequest>? ImagesToUpdate { get; set; }
    public List<string>? ImagesToDelete { get; set; }
    public IFormFile? NewMainImage { get; set; }
    public List<IFormFile>? NewExteriorImages { get; set; }
    public List<IFormFile>? NewInteriorImages { get; set; }
    public List<IFormFile>? NewOtherImages { get; set; }
}
public class ImageUpdateRequest
{
    public string? ImageUrl { get; set; }
    public int OrderNumber { get; set; }
    public ImageCategory NewCategory { get; set; }
}
public class UpdateCarCommandHandler(
    IGenericRepository<Car> carRepository,
    IGenericRepository<CarImage> carImageRepository,
    IMapper mapper,
    IFileService fileService
) : IRequestHandler<UpdateCarCommand>
{
    public async Task Handle(UpdateCarCommand cmd, CancellationToken cancellationToken)
    {
        var existingCar = await carRepository.GetByIdAsync(cmd.Id)
            ?? throw new HttpException($"Car with id [{cmd.Id}] not found!", HttpStatusCode.NotFound);

        mapper.Map(cmd, existingCar);
        await carRepository.UpdateAsync(existingCar);

        if (cmd.ImagesToDelete != null && cmd.ImagesToDelete.Any())
        {
            var spec = new CarImagesByCarIdAndUrlsSpec(cmd.Id, cmd.ImagesToDelete);
            var imagesToDelete = await carImageRepository.GetListBySpec<CarImage>(spec, cancellationToken);

            if (imagesToDelete.Any())
            {
                await fileService.DeleteImagesByUrlsAsync(imagesToDelete.Select(img => img.ImageUrl).ToList()!);

                foreach (var image in imagesToDelete)
                {
                    await carImageRepository.DeleteAsync(image.Id);
                }
            }
        }

        if (cmd.ImagesToUpdate != null && cmd.ImagesToUpdate.Any())
        {
            foreach (var update in cmd.ImagesToUpdate)
            {
                var spec = new CarImageByCarIdAndUrlSpec(cmd.Id, update.ImageUrl!);
                var image = await carImageRepository.GetItemBySpec<CarImage>(spec, cancellationToken)
                    ?? throw new HttpException($"Image with URL [{update.ImageUrl}] not found!", HttpStatusCode.NotFound);

                image.OrderNumber = update.OrderNumber;
                image.ImageCategory = update.NewCategory;
                await carImageRepository.UpdateAsync(image);
            }
        }

        var maxOrderSpec = new CarImagesMaxOrderNumberSpec(cmd.Id);
        int maxOrderNumber = (await carImageRepository.GetListBySpec<int>(maxOrderSpec, cancellationToken))
            .DefaultIfEmpty(0)
            .Max() + 1;

        if (cmd.NewMainImage != null)
        {
            var imageUrl = await fileService.UploadImageAsync(cmd.NewMainImage);
            await carImageRepository.InsertAsync(new CarImage
            {
                CarId = cmd.Id,
                ImageUrl = imageUrl,
                ImageCategory = ImageCategory.Main,
                OrderNumber = maxOrderNumber++,
                UploadedAt = DateTime.UtcNow
            });
        }

        if (cmd.NewExteriorImages != null && cmd.NewExteriorImages.Any())
        {
            var imageUrls = await fileService.UploadImagesAsync(cmd.NewExteriorImages);
            foreach (var url in imageUrls)
            {
                await carImageRepository.InsertAsync(new CarImage
                {
                    CarId = cmd.Id,
                    ImageUrl = url,
                    ImageCategory = ImageCategory.Exterior,
                    OrderNumber = maxOrderNumber++,
                    UploadedAt = DateTime.UtcNow
                });
            }
        }

        if (cmd.NewInteriorImages != null && cmd.NewInteriorImages.Any())
        {
            var imageUrls = await fileService.UploadImagesAsync(cmd.NewInteriorImages);
            foreach (var url in imageUrls)
            {
                await carImageRepository.InsertAsync(new CarImage
                {
                    CarId = cmd.Id,
                    ImageUrl = url,
                    ImageCategory = ImageCategory.Interior,
                    OrderNumber = maxOrderNumber++,
                    UploadedAt = DateTime.UtcNow
                });
            }
        }

        if (cmd.NewOtherImages != null && cmd.NewOtherImages.Any())
        {
            var imageUrls = await fileService.UploadImagesAsync(cmd.NewOtherImages);
            foreach (var url in imageUrls)
            {
                await carImageRepository.InsertAsync(new CarImage
                {
                    CarId = cmd.Id,
                    ImageUrl = url,
                    ImageCategory = ImageCategory.Other,
                    OrderNumber = maxOrderNumber++,
                    UploadedAt = DateTime.UtcNow
                });
            }
        }
    }
}

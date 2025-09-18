using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Exceptions;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Cars;

public class RequestNewCarCommand : IRequest
{
    public string fullName { get; set; }
    public string phone { get; set; }
    public string vin { get; set; }
    public int brandId { get; set; }
    public int modelId { get; set; }
    public int transmissionId { get; set; }
    public int year { get; set; }
    public int mileage { get; set; }
    public string description { get; set; }
    public bool isOnSaleElsewhere { get; set; }
    public bool isModified { get; set; }
    public List<IFormFile> photos { get; set; }
    public int userId { get; set; } 
}

public class RequestNewCarCommandHandler(UserManager<User> userManager,
                                         IGenericRepository<Car> carRepository,
                                         IMapper mapper,
                                         IFileService fileService,
                                         IGenericRepository<CarImage> carImageRepository) : IRequestHandler<RequestNewCarCommand>
{
    public async Task Handle(RequestNewCarCommand request, CancellationToken cancellationToken)
    {
        var car = mapper.Map<Car>(request);
        car.Status = CarStatus.inPending;
        
        var user = await userManager.FindByIdAsync(request.userId.ToString());
        if (user == null)
        {
            throw new HttpException("User not found", HttpStatusCode.NotFound);
        }
        user.FullName = request.fullName;
        user.PhoneNumber = request.phone;
        var updateResult = await userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            throw new HttpException("Failed to update user: " + string.Join(", ", updateResult.Errors.Select(e => e.Description)), HttpStatusCode.Conflict);
        }

        car.OwnerId = request.userId;

        await carRepository.InsertAsync(car);

        int orderNumber = 0;
        
        var ImageUrls = await fileService.UploadImagesAsync(request.photos);
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
}
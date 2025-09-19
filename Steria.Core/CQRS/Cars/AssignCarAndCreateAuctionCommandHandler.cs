using MediatR;
using Steria.Core.CQRS.Auctions;
using Steria.Core.CQRS.Chat;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Exceptions;
using Steria.Core.Interfaces;
using Steria.Core.Resources;
using Steria.Core.Specification.CarSpec;
using System.Net;

namespace Steria.Core.CQRS.Cars;

public class AssignCarAndCreateAuctionCommand : IRequest
{
    public int CarId { get; set; }
    public int ManagerId { get; set; }
}

public class AssignCarAndCreateAuctionCommandHandler(
    IGenericRepository<Car> carRepository,
    IGenericRepository<CarImage> carImageRepository,
    IMediator mediator,
    IFileService fileService
) : IRequestHandler<AssignCarAndCreateAuctionCommand>
{
    public async Task Handle(AssignCarAndCreateAuctionCommand cmd, CancellationToken cancellationToken)
    {
        var car = await carRepository.GetByIdAsync(cmd.CarId)
            ?? throw new HttpException(string.Format(Resource.CarNotFoundById, cmd.CarId), HttpStatusCode.NotFound);

        //if (car.Status != CarStatus.inPending)
        //{
        //    throw new HttpException("Car must be in pending status to assign and start auction.", HttpStatusCode.BadRequest);
        //}

        if (car.ManagerId != null)
        {
            throw new HttpException("Car is already assigned to a manager.", HttpStatusCode.BadRequest);
        }

        // 1. Update ManagerId and Status to inReview
        car.ManagerId = cmd.ManagerId;
        car.Status = CarStatus.inReview;
        await carRepository.UpdateAsync(car);

        // 2. Delete all photos from this car
        var imagesSpec = new CarImagesObjectByCarIdSpec(cmd.CarId); // Ensure spec returns List<CarImage>
        var images = await carImageRepository.GetListBySpec(imagesSpec, cancellationToken);
        if (images.Any())
        {
            var imageUrls = images.Select(img => img.ImageUrl!).ToList(); // CarImage has ImageUrl property
            await fileService.DeleteImagesByUrlsAsync(imageUrls);
            foreach (var image in images)
            {
                await carImageRepository.DeleteAsync(image.Id); // CarImage has Id property
            }
        }

        // 3. Create auction with status New
        var startTime = DateTime.UtcNow;
        var endTime = startTime.AddDays(7);
        var createAuctionCommand = new CreateAuctionCommand
        {
            CarId = car.Id,
            SellerId = car.OwnerId,
            StartPrice = 1000m,
            StartTime = startTime,
            EndTime = endTime,
            Status = AuctionStatus.New // Ensure AuctionStatus.New exists in your enum
        };
        await mediator.Send(createAuctionCommand);

        // 4. Status change already handled in step 1

        // 5. Auction status set to New in step 3

        // 6. Create chat between owner (seller) and manager
        var createChatCommand = new CreateChatCommand
        {
            ParticipantIds = new List<int> { car.OwnerId, cmd.ManagerId }
        };
        var chatId = await mediator.Send(createChatCommand);

        // Update car with chat_id
        car.ChatId = chatId;
        await carRepository.UpdateAsync(car);
    }
}
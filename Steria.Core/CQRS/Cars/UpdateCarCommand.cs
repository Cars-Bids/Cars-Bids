using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Exceptions;
using Steria.Core.Interfaces;
using Steria.Core.Resources;
using Steria.Core.Specification.CarSpec;

namespace Steria.Core.CQRS.Cars;

public class UpdateCarCommand : IRequest
{
    public int Id { get; set; }
    public int ModelId { get; set; }
    public int Mileage { get; set; }
    public int Year { get; set; }
    public string Vin { get; set; } = null!;
    public string? Location { get; set; }
    public string? ExteriorColor { get; set; }
    public string? InteriorColor { get; set; }
    public string? Engine { get; set; }
    public int? DrivetrainId { get; set; }
    public int TransmissionId { get; set; }
    public int? BodyStyleId { get; set; }
    public int? Speeds { get; set; }
    public string? Highlights { get; set; }
    public string? ServiceHistory { get; set; }
    public string? Equipment { get; set; }
    public string? Flaws { get; set; }
    public string? Modifications { get; set; }
    public string? OtherItems { get; set; }
    public string? OwnershipHistory { get; set; }
    public string? SellerNotes { get; set; }
    public string? VideoLinks { get; set; }
    
    // auction part

    public int AuctionId { get; set; }
    public decimal? StartPrice { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public bool? IsInspected { get; set; }
    
}
public class UpdateCarCommandHandler(
    IGenericRepository<Car> carRepository,
    IGenericRepository<Auction> auctionRepository,
    IMapper mapper
) : IRequestHandler<UpdateCarCommand>
{
    public async Task Handle(UpdateCarCommand cmd, CancellationToken cancellationToken)
    {
        var existingCar = await carRepository.GetByIdAsync(cmd.Id)
            ?? throw new HttpException(
                string.Format(Resource.CarNotFoundById, cmd.Id), HttpStatusCode.NotFound);

        mapper.Map(cmd, existingCar);
        await carRepository.UpdateAsync(existingCar);

        var existingAuction = await auctionRepository.GetByIdAsync(cmd.AuctionId)
                              ?? throw new HttpException("Auction not found by id.", HttpStatusCode.NotFound);

        existingAuction.StartPrice = cmd.StartPrice;
        existingAuction.StartTime = cmd.StartTime;
        existingAuction.EndTime = cmd.EndTime;
        existingAuction.IsInspected = (bool)cmd.IsInspected;
        
        await auctionRepository.UpdateAsync(existingAuction);
    }
}

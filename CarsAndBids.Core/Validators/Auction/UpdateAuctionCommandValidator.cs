using FluentValidation;
using CarsAndBids.Core.CQRS.Auctions;
using CarsAndBids.Core.Resources;

namespace CarsAndBids.Core.Validators.Auction;

public class UpdateAuctionCommandValidator : AbstractValidator<UpdateAuctionCommand>
{
    public UpdateAuctionCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage(Resource.AuctionIdGreaterThanZero);

        RuleFor(x => x.CarId)
            .GreaterThan(0).WithMessage(Resource.CarIdGreaterThanZero);

        RuleFor(x => x.SellerId)
            .GreaterThan(0).WithMessage(Resource.SellerIdGreaterThanZero);

        RuleFor(x => x.StartPrice)
            .GreaterThan(0).WithMessage(Resource.StartPriceGreaterThanZero);

        RuleFor(x => x.CurrentPrice)
            .GreaterThanOrEqualTo(x => x.StartPrice)
            .WithMessage(Resource.CurrentPriceGTEStartPrice);

        RuleFor(x => x.StartTime)
            .LessThan(x => x.EndTime)
            .WithMessage(Resource.StartTimeBeforeEndTime);

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage(Resource.InvalidAuctionStatus);
    }
}

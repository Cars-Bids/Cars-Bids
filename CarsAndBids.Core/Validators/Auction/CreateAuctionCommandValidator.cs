using FluentValidation;
using CarsAndBids.Core.CQRS.Auctions;
using CarsAndBids.Core.Resources;

namespace CarsAndBids.Core.Validators.Auction;

public class CreateAuctionCommandValidator : AbstractValidator<CreateAuctionCommand>
{
    public CreateAuctionCommandValidator()
    {
        RuleFor(x => x.CarId)
            .GreaterThan(0).WithMessage(Resource.CarIdGreaterThanZero);

        RuleFor(x => x.SellerId)
            .GreaterThan(0).WithMessage(Resource.SellerIdGreaterThanZero);

        RuleFor(x => x.StartPrice)
            .GreaterThan(0).WithMessage(Resource.StartPriceGreaterThanZero);

        RuleFor(x => x.StartTime)
            .GreaterThan(DateTime.UtcNow).WithMessage(Resource.StartTimeInFuture);

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime).WithMessage(Resource.EndTimeAfterStartTime);
    
    }
}

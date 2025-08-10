using FluentValidation;
using Steria.Core.CQRS.Auctions;

namespace Steria.Core.Validators.Auction;

public class CreateAuctionCommandValidator : AbstractValidator<CreateAuctionCommand>
{
    public CreateAuctionCommandValidator()
    {
        RuleFor(x => x.CarId)
            .GreaterThan(0).WithMessage("Car ID must be greater than 0");

        RuleFor(x => x.SellerId)
            .GreaterThan(0).WithMessage("Seller ID must be greater than 0");

        RuleFor(x => x.StartPrice)
            .GreaterThan(0).WithMessage("Start price must be greater than 0");

        RuleFor(x => x.StartTime)
            .GreaterThan(DateTime.UtcNow).WithMessage("Start time must be in the future");

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time");
    }
}

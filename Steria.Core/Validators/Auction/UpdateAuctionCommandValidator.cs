using FluentValidation;
using Steria.Core.CQRS.Auctions;

namespace Steria.Core.Validators.Auction;

public class UpdateAuctionCommandValidator : AbstractValidator<UpdateAuctionCommand>
{
    public UpdateAuctionCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Auction ID must be greater than 0");

        RuleFor(x => x.CarId)
            .GreaterThan(0).WithMessage("Car ID must be greater than 0");

        RuleFor(x => x.SellerId)
            .GreaterThan(0).WithMessage("Seller ID must be greater than 0");

        RuleFor(x => x.StartPrice)
            .GreaterThan(0).WithMessage("Start price must be greater than 0");

        RuleFor(x => x.CurrentPrice)
            .GreaterThanOrEqualTo(x => x.StartPrice)
            .WithMessage("Current price can't be less than start price");

        RuleFor(x => x.StartTime)
            .LessThan(x => x.EndTime)
            .WithMessage("Start time must be before end time");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Invalid auction status");
    }
}

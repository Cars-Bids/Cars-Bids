using Ardalis.Specification;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Enums;

namespace Steria.Core.Specification.ProfileSpec;

public class UserWishlistSpec : Specification<Wishlist, WishlistItemDto>
{
    public UserWishlistSpec(int userId, int pageNumber, int pageSize)
    {
        Query
            .Where(w => w.UserId == userId)
            .Include(w => w.Auction)
                .ThenInclude(a => a.Car)
                    .ThenInclude(c => c.Model)
                        .ThenInclude(m => m.Make)
            .Include(w => w.Auction)
                .ThenInclude(a => a.Car)
                    .ThenInclude(c => c.BodyStyle)
            .Include(w => w.Auction)
                .ThenInclude(a => a.Car)
                    .ThenInclude(c => c.Images)
            .OrderBy(w => w.AddedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking();

        Query.Select(w => new WishlistItemDto
        {
            AuctionId = w.Auction.Id,
            StartPrice = w.Auction.StartPrice,
            CurrentPrice = w.Auction.CurrentPrice,
            StartTime = w.Auction.StartTime,
            EndTime = w.Auction.EndTime,
            Status = w.Auction.Status.ToString(),
            CarId = w.Auction.Car.Id,
            CarName = $"{w.Auction.Car.Model.Make.Name} {w.Auction.Car.Model.Name}",
            Year = w.Auction.Car.Year,
            Make = w.Auction.Car.Model.Make.Name,
            Model = w.Auction.Car.Model.Name,
            BodyStyle = w.Auction.Car.BodyStyle.StyleName,
            ExteriorColor = w.Auction.Car.ExteriorColor,
            InteriorColor = w.Auction.Car.InteriorColor,
            Engine = w.Auction.Car.Engine,
            Drivetrain = w.Auction.Car.Drivetrain.ToString(),
            TransmissionType = w.Auction.Car.TransmissionType.ToString(),
            MainImage = w.Auction.Car.Images
                .Where(img => img.ImageCategory == ImageCategory.Main || img.OrderNumber == 1)
                .Select(img => img.ImageUrl)
                .FirstOrDefault(),
            AddedAt = w.AddedAt
        });
    }
}
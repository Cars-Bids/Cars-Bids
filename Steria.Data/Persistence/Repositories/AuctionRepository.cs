using Microsoft.EntityFrameworkCore;
using Steria.Core.DTOs;
using Steria.Core.Enums;
using Steria.Core.Interfaces;

namespace Steria.Data.Persistence.Repositories;

public class AuctionRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory) : IAuctionRepository
{
    public async Task<AuctionData?> GetAuctionByIdAsync(int auctionId, int userId)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();

        var res = await context.Auctions.AsNoTracking()
            .Where(x => x.Id == auctionId)
            .Select(x => new AuctionData
            {
                Id = x.Id,
                CarId = x.CarId,
                Status = x.Status.ToString(),
                BidsCount = x.Bids!.Count,
                Seller = x.Seller.UserName,
                SellerPhoto = x.Seller.ProfilePictureUrl,
                CurrentBidder = x.CurrentBidder,
                CurrentBidderPhoto = x.Bids!.OrderBy(x => x.BidAmount).LastOrDefault()!.User.ProfilePictureUrl,
                CurrentPrice = x.CurrentPrice,
                EndTime = x.EndTime,
                WatchersCount = x.Wishlists!.Count,
                ViewsCount = 0,
                IsSeller = x.Seller.Id == userId,
                IsInspected = x.IsInspected,
                IsWatched = x.Wishlists!.Any(x => x.UserId == userId)
            })
            .FirstOrDefaultAsync();

        return res;
    }

    public async Task<CarData?> GetAuctionCarByIdAsync(int carId)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();

        var res = await context.Cars.AsNoTracking()
            .Where(x => x.Id == carId)
            .Select(x => new CarData
            {
                Id = x.Id,
                ExteriorColor = x.ExteriorColor,
                Seller = x.Owner.UserName,
                BodyStyle = x.BodyStyle!.StyleName,
                Drivetrain = x.Drivetrain.ToString(),
                Brand = x.Model.Make.Name,
                Engine = x.Engine,
                Equipment = x.Equipment!.Split('\n', StringSplitOptions.TrimEntries),
                Flaws = x.Flaws!.Split('\n', StringSplitOptions.TrimEntries),
                Highlights = x.Highlights!.Split('\n', StringSplitOptions.TrimEntries),
                SellerNotes = x.SellerNotes!.Split('\n', StringSplitOptions.TrimEntries),
                InteriorColor = x.InteriorColor,
                SellerPhoto = x.Owner.ProfilePictureUrl,
                Location = x.Location,
                SellerType = "Dealer",
                OwnershipHistory = x.OwnershipHistory!.Split('\n', StringSplitOptions.TrimEntries),
                Mileage = x.Mileage,
                ServiceHistory = x.ServiceHistory!.Split('\n', StringSplitOptions.TrimEntries),
                Model = x.Model.Name,
                Modifications = x.Modifications!.Split('\n', StringSplitOptions.TrimEntries),
                TitleStatus = "Clean (WA)",
                OtherItems = x.OtherItems!.Split('\n', StringSplitOptions.TrimEntries),
                TransmissionType = x.TransmissionType.ToString(),
                VideoLinks = x.VideoLinks!.Split(',', StringSplitOptions.TrimEntries),
                Vin = x.Vin,
                Year = x.Year,
                Title = $"{x.Year} {x.Model.Make.Name} {x.Model.Name}",
                Subtitle = $"{x.Engine}, {x.Mileage} Miles, {x.Model.Name}, {x.TransmissionType.ToString()}, {x.Drivetrain.ToString()}"
            })
            .FirstOrDefaultAsync();

        return res;
    }

    public async Task<List<CarImageData>> GetCarImagesByCarIdAsync(int carId)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();

        var res = await context.CarImages.AsNoTracking()
            .Where(x => x.CarId == carId)
            .OrderBy(x => x.ImageCategory)
            .ThenBy(x => x.OrderNumber)
            .Select(x => new CarImageData
            {
                Id = x.Id,
                ImageUrl = x.ImageUrl,
                ImageCategory = x.ImageCategory.ToString()
            })
            .ToListAsync();

        return res;
    }

    public async Task<List<QAData>> GetQaByAuctionIdAsync(int auctionId)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();

        var res = await context.Questions.AsNoTracking()
            .Where(x => x.AuctionId == auctionId)
            .OrderByDescending(x => x.CreatedAt)
            .Take(10)
            .Select(x => new QAData
            {
                Id = x.Id,
                Question = x.QuestionText,
                Answer = x.Answer!.AnswerText,
                Author = x.User!.UserName,
                AuthorPhoto = x.User.ProfilePictureUrl
            })
            .ToListAsync();

        return res;
    }

    public async Task<List<CommentData>> GetCommentsByAuctionIdAsync(int auctionId)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();

        var res = await context.Comments.AsNoTracking()
            .Where(x => x.AuctionId == auctionId)
            .OrderByDescending(x => x.CreatedAt)
            .Take(10)
            .Select(x => new CommentData
            {
                Id = x.Id,
                Author = x.User!.UserName,
                AuthorPhoto = x.User.ProfilePictureUrl,
                CreatedAt = x.CreatedAt,
                Text = x.Text,
                ReplyTo = x.ReplyedTo!.User.UserName
            })
            .ToListAsync();

        return res;
    }

    public async Task<List<OtherAuction>> GetOtherAuctionsAsync(int auctionId, int userId)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();

        var res = await context.Auctions.AsNoTracking()
            .Where(x => x.Id != auctionId)
            .Where(x => x.Status == AuctionStatus.Active || x.Status == AuctionStatus.Pending)
            .OrderBy(x => x.EndTime)
            .Take(10)
            .Select(x => new OtherAuction
            {
                Id = x.Id,
                MainPhoto = x.Car.Images!.OrderBy(x => x.ImageCategory).ThenBy(x => x.OrderNumber).FirstOrDefault()!.ImageUrl,
                CurrentPrice = x.CurrentPrice,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                IsInspected = x.IsInspected,
                IsWatched = x.Wishlists!.Any(x => x.UserId == userId),
                Location = x.Car.Location,
                Title = $"{x.Car.Year} {x.Car.Model.Make.Name} {x.Car.Model.Name}",
                Subtitle = $"{x.Car.Engine}, {x.Car.Mileage} Miles, {x.Car.Model.Name}, {x.Car.TransmissionType.ToString()}, {x.Car.Drivetrain.ToString()}"
            })
            .ToListAsync();

        return res;
    }
}

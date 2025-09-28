using AutoMapper;
using Steria.Core.CQRS.Account;
using Steria.Core.CQRS.Auctions;
using Steria.Core.CQRS.BodyStyles;
using Steria.Core.CQRS.Cars;
using Steria.Core.CQRS.Chat;
using Steria.Core.CQRS.Comments;
using Steria.Core.CQRS.Makes;
using Steria.Core.CQRS.Models;
using Steria.Core.CQRS.NotificationTypes;
using Steria.Core.CQRS.Profile;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Enums;

namespace Steria.Core.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Auction, AuctionDto>()
            .ForMember(dest => dest.Car, opt => opt.MapFrom(src => src.Car));
        CreateMap<Auction, CreateAuctionCommand>().ReverseMap();
        CreateMap<Auction, UpdateAuctionCommand>().ReverseMap();
        CreateMap<Auction, AuctionWithCarDto>()
            .ForMember(dest => dest.Car, opt => opt.MapFrom(src => src.Car))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ReverseMap();
        CreateMap<Auction, AuctionWithCarDtoNewest>()
            .ForMember(dest => dest.Car, opt => opt.MapFrom(src => src.Car))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ReverseMap();
        CreateMap<Auction, ManagingAuctionPageDto>()
            .ForMember(dest => dest.CarBrandId,
                opt => opt.MapFrom(src => src.Car.Model.MakeId))
            .ForMember(dest => dest.CarDriveTrainId,
                opt => opt.MapFrom(src => src.Car.Drivetrain.HasValue
                    ? (int?)src.Car.Drivetrain.Value
                    : null))
            .ForMember(dest => dest.CarTransmissionId,
                opt => opt.MapFrom(src => (int)src.Car.TransmissionType))
            .ForMember(dest => dest.CarBodyStyleId,
                opt => opt.MapFrom(src => src.Car.BodyStyleId));
        
        CreateMap<BodyStyle, BodyStyleDto>().ReverseMap();
        CreateMap<BodyStyle, UpdateBodyStyleCommand>().ReverseMap();
        CreateMap<BodyStyle, CreateBodyStyleCommand>().ReverseMap();

        CreateMap<Make, MakeDto>().ReverseMap();
        CreateMap<Make, UpdateMakeCommand>().ReverseMap();
        CreateMap<Make, CreateMakeCommand>().ReverseMap();

        CreateMap<Comment, CommentDto>().ReverseMap();
        CreateMap<Comment, UpdateCommentCommand>().ReverseMap();
        CreateMap<Comment, CreateCommentCommand>().ReverseMap();
        CreateMap<Comment, CommentWithNameDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));

        CreateMap<Model, ModelDto>().ReverseMap();
        CreateMap<Model, UpdateModelCommand>().ReverseMap();
        CreateMap<Model, CreateModelCommand>().ReverseMap();

        CreateMap<CarImage, CarImageDto>().ReverseMap();

        CreateMap<Car, CarDto>().ReverseMap();
        CreateMap<UpdateCarCommand, Car>().ForMember(dest => dest.Auction, opt => opt.Ignore());
        CreateMap<Car, CreateCarCommand>().ReverseMap();
        CreateMap<Car, CarNewestDto>()
            .ForMember(dest => dest.MainImage, opt => opt.MapFrom(src =>
                src.Images != null && src.Images.Any(img => img.ImageCategory == ImageCategory.Main)
                    ? src.Images.First(img => img.ImageCategory == ImageCategory.Main).ImageUrl
                    : "https://wsa3.pakwheels.com/assets/default-display-image-car-6873f23250596c4daa082e7223e5bbb5d1fbcaf7bb5d7113003daa9ebd3c66a8.png"))
            .ForMember(dest => dest.ExteriorImage1, opt => opt.MapFrom(src =>
                src.Images != null && src.Images.Any(img => img.ImageCategory == ImageCategory.Exterior)
                    ? src.Images.Where(img => img.ImageCategory == ImageCategory.Exterior)
                               .OrderBy(img => img.OrderNumber)
                               .Select(img => img.ImageUrl)
                               .FirstOrDefault()
                    : "https://wsa3.pakwheels.com/assets/default-display-image-car-6873f23250596c4daa082e7223e5bbb5d1fbcaf7bb5d7113003daa9ebd3c66a8.png"))
            .ForMember(dest => dest.ExteriorImage2, opt => opt.MapFrom(src =>
                src.Images != null && src.Images.Any(img => img.ImageCategory == ImageCategory.Exterior)
                    ? src.Images.Where(img => img.ImageCategory == ImageCategory.Exterior)
                               .OrderBy(img => img.OrderNumber)
                               .Skip(1)
                               .Select(img => img.ImageUrl)
                               .FirstOrDefault()
                    : "https://wsa3.pakwheels.com/assets/default-display-image-car-6873f23250596c4daa082e7223e5bbb5d1fbcaf7bb5d7113003daa9ebd3c66a8.png"))
            .ForMember(dest => dest.MakeName, opt => opt.MapFrom(src => src.Model != null && src.Model.Make != null ? src.Model.Make.Name : null))
            .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.Model != null ? src.Model.Name : null));

        CreateMap<User, ProfileDto>()
            .ForMember(dest => dest.FollowersCount, opt => opt.MapFrom(src => src.Followers.Count))
            .ForMember(dest => dest.FollowingCount, opt => opt.MapFrom(src => src.Following.Count))
            .ReverseMap();

        CreateMap<User, ProfileDto>()
            .ForMember(dest => dest.FollowersCount, opt => opt.MapFrom(src => src.Followers.Count))
            .ForMember(dest => dest.FollowingCount, opt => opt.MapFrom(src => src.Following.Count))
            .ReverseMap();

        CreateMap<UpdateProfileCommand, User>()
            .ForMember(dest => dest.ProfilePictureUrl, opt => opt.Ignore());

        CreateMap<NotificationType, CreateNotificationTypeCommand>().ReverseMap();
        CreateMap<NotificationType, UpdateNotificationTypeCommand>().ReverseMap();
        CreateMap<NotificationType, NotificationTypeDto>().ReverseMap();
        CreateMap<UserNotificationSetting, UserNotificationSettingDto>().ReverseMap();
            //.ForMember(dest => dest.NotificationType, opt => opt.MapFrom(src => src.NotificationType));

        CreateMap<SendChatMessageCommand, ChatMessage>();
        CreateMap<ChatRequirements, ChatRequirementDto>();

        CreateMap<ChatMessage, ChatMessageDto>()
            .ForMember(dest => dest.Attachment,
                opt => opt.MapFrom(src => src.Attachments != null
                    ? src.Attachments.Select(a => a.ImageUrl).ToList()
                    : new List<string>()))
            .ForMember(dest => dest.SentAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.ReactionSummaryDtos, opt => opt.Ignore())
            .AfterMap((src, dest, ctx) =>
            {
                var currentUserId = ctx.Items.TryGetValue("UserId", out object? value) ? (int)value : 0;
                var isOwner = src.SenderId == currentUserId;

                // mapping grouped reactions
                var grouped = src.UserChatMessageReactions?
                    .SelectMany(r => r.EmojiReactions.Select(e => new { e.Emoji, UserId = r.UserId }))
                    .GroupBy(x => x.Emoji)
                    .Select(g => new ReactionSummaryDto
                    {
                        Emoji = g.Key,
                        Count = g.Count(),
                        ReactedByCurrentUser = g.Any(x => x.UserId == currentUserId)
                    }).ToList();

                dest.ReactionSummaryDtos = grouped;

                // if current user - sender, adding seenBy
                if (isOwner)
                {
                    dest.SeenBy = src.UserChatMessageReactions?
                        .Where(r => r.SeenAt != default)
                        .Select(r => new SeenInfoDto
                        {
                            UserId = r.UserId,
                            SeenAt = r.SeenAt
                        }).ToList();
                }
            });

        CreateMap<RegisterCommand, User>();

        CreateMap<Bid, UserBiddedCarsDto>()
            .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.Auction.CarId))
            .ForMember(dest => dest.CarName, opt => opt.MapFrom(src => $"{src.Auction.Car.Year} {src.Auction.Car.Model.Make.Name} {src.Auction.Car.Model.Name}"))
            .ForMember(dest => dest.Engine, opt => opt.MapFrom(src => src.Auction.Car.Engine))
            .ForMember(dest => dest.Drivetrain, opt => opt.MapFrom(src => src.Auction.Car.Drivetrain.ToString()))
            .ForMember(dest => dest.Transmission, opt => opt.MapFrom(src => src.Auction.Car.TransmissionType.ToString()))
            .ForMember(dest => dest.BodyStyle, opt => opt.MapFrom(src => src.Auction.Car.BodyStyle.ToString()))
            .ForMember(dest => dest.ExteriorColor, opt => opt.MapFrom(src => src.Auction.Car.ExteriorColor))
            .ForMember(dest => dest.InteriorColor, opt => opt.MapFrom(src => src.Auction.Car.InteriorColor));

        CreateMap<Comment, UserCommentDto>()
            .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.Auction.CarId))
            .ForMember(dest => dest.CarName, opt => opt.MapFrom(src => $"{src.Auction.Car.Year} {src.Auction.Car.Model.Make.Name} {src.Auction.Car.Model.Name}"))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Auction.Car.Year))
            .ForMember(dest => dest.Make, opt => opt.MapFrom(src => src.Auction.Car.Model.Make.Name))
            .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Auction.Car.Model.Name))
            .ForMember(dest => dest.BodyStyle, opt => opt.MapFrom(src => src.Auction.Car.BodyStyle))
            .ForMember(dest => dest.MainImage, opt => opt.MapFrom(src => src.Auction.Car.Images
                .Where(img => img.ImageCategory == ImageCategory.Main || img.OrderNumber == 1)
                .Select(img => img.ImageUrl)
                .FirstOrDefault()));

        CreateMap<Wishlist, WishlistItemDto>()
            //.ForMember(dest => dest.CarName, opt => opt.MapFrom(src => $"{src.Auction.Car.Model.Make.Name} {src.Auction.Car.Model.Name}"))
            .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Auction.Car.Year))
            .ForMember(dest => dest.Make, opt => opt.MapFrom(src => src.Auction.Car.Model.Make.Name))
            .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Auction.Car.Model.Name))
            .ForMember(dest => dest.BodyStyle, opt => opt.MapFrom(src => src.Auction.Car.BodyStyle))
            .ForMember(dest => dest.ExteriorColor, opt => opt.MapFrom(src => src.Auction.Car.ExteriorColor))
            .ForMember(dest => dest.InteriorColor, opt => opt.MapFrom(src => src.Auction.Car.InteriorColor))
            .ForMember(dest => dest.Engine, opt => opt.MapFrom(src => src.Auction.Car.Engine))
            .ForMember(dest => dest.Drivetrain, opt => opt.MapFrom(src => src.Auction.Car.Drivetrain))
            .ForMember(dest => dest.TransmissionType, opt => opt.MapFrom(src => src.Auction.Car.TransmissionType))
            .ForMember(dest => dest.MainImage, opt => opt.MapFrom(src => src.Auction.Car.Images
                .Where(img => img.ImageCategory == ImageCategory.Main || img.OrderNumber == 1)
                .Select(img => img.ImageUrl)
                .FirstOrDefault()))
            .ForMember(dest => dest.AddedAt, opt => opt.MapFrom(src => src.AddedAt));


        CreateMap<UserNotification, UserNotificationDto>()
            .ForMember(dest => dest.TypeKey, opt => opt.MapFrom(src => src.NotificationType.Key));


        CreateMap<Car, CarPreviewDto>()
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src =>
                $"{src.Engine}, {src.Drivetrain}, {(src.Speeds.HasValue ? $"{src.Speeds}-speed " : "")}{src.TransmissionType}"))
            .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model.Name))
            .ForMember(dest => dest.Make, opt => opt.MapFrom(src => src.Model.Make.Name))
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src =>
                src.Images != null && src.Images.Any(img => img.ImageCategory == ImageCategory.Main)
                    ? src.Images.First(img => img.ImageCategory == ImageCategory.Main).ImageUrl
                    : null));

        CreateMap<Comment, AuctionActivityDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(_ => "Comment"))
            .ForMember(dest => dest.Upvotes, opt => opt.MapFrom(src => src.CommentUpvotes.Count));

        CreateMap<Bid, AuctionActivityDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(_ => "Bid"))
            .ForMember(dest => dest.BidderId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.BidderName, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.BidAmount));
        
        CreateMap<RequestNewCarCommand, Car>()
            .ForMember(dest => dest.TransmissionType, opt => opt.MapFrom(src => (TransmissionType)src.transmissionId))
            .ForMember(dest => dest.Images, opt => opt.Ignore());

        CreateMap<Auction, AuctionWithCarDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<Car, ProfileEndedCarDto>()
            .ForMember(dest => dest.Make, opt => opt.MapFrom(src => src.Model.Make.Name))
            .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model.Name))
            .ForMember(dest => dest.BodyStyle, opt => opt.MapFrom(src => src.BodyStyle.StyleName))
            //.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.MainImage, opt => opt.MapFrom(src => src.Images
                .Where(img => img.ImageCategory == ImageCategory.Main || img.OrderNumber == 1)
                .Select(img => img.ImageUrl)
                .FirstOrDefault() ?? "https://wsa3.pakwheels.com/assets/default-display-image-car-6873f23250596c4daa082e7223e5bbb5d1fbcaf7bb5d7113003daa9ebd3c66a8.png"));

        CreateMap<Car, ProfileInReviewCarDto>()
            .ForMember(dest => dest.Make, opt => opt.MapFrom(src => src.Model.Make.Name))
            .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model.Name))
            .ForMember(dest => dest.BodyStyle, opt => opt.MapFrom(src => src.BodyStyle.StyleName))
            .ForMember(dest => dest.OtherImage, opt => opt.MapFrom(src => src.Images
                .Where(img => img.ImageCategory == ImageCategory.Other)
                .OrderBy(img => img.OrderNumber)
                .Select(img => img.ImageUrl)
                .FirstOrDefault() ?? "https://wsa3.pakwheels.com/assets/default-display-image-car-6873f23250596c4daa082e7223e5bbb5d1fbcaf7bb5d7113003daa9ebd3c66a8.png"))
            .ForMember(dest => dest.Auction, opt => opt.MapFrom(src => src.Auction))
            .ForMember(dest => dest.ChatId, opt => opt.MapFrom(src => src.ChatId));

        CreateMap<Car, CarManagerDto>()
            .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner.UserName))
            .ForMember(dest => dest.BodyStyle, opt => opt.MapFrom(src => src.BodyStyle != null ? src.BodyStyle.StyleName : string.Empty))
            .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model != null ? src.Model.Name : string.Empty))
            .ForMember(dest => dest.Make, opt => opt.MapFrom(src => src.Model.Make != null ? src.Model.Make.Name : string.Empty))
            .ReverseMap();

        CreateMap<Answer, AddAnswerCommand>().ReverseMap();
        CreateMap<Question, AddQuestionCommand>().ReverseMap();

    }
}

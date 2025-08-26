using AutoMapper;
using CarsAndBids.Core.CQRS.Account;
using CarsAndBids.Core.CQRS.Auctions;
using CarsAndBids.Core.CQRS.BodyStyles;
using CarsAndBids.Core.CQRS.Cars;
using CarsAndBids.Core.CQRS.Chat;
using CarsAndBids.Core.CQRS.Comments;
using CarsAndBids.Core.CQRS.Makes;
using CarsAndBids.Core.CQRS.Models;
using CarsAndBids.Core.CQRS.Profile;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Enums;

namespace CarsAndBids.Core.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Auction, AuctionDto>().ReverseMap();
        CreateMap<Auction, CreateAuctionCommand>().ReverseMap();
        CreateMap<Auction, UpdateAuctionCommand>().ReverseMap();

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
        CreateMap<Car, UpdateCarCommand>().ReverseMap();
        CreateMap<Car, CreateCarCommand>().ReverseMap();

        CreateMap<User, ProfileDto>().ReverseMap();
        CreateMap<UpdateProfileCommand, User>()
            .ForMember(dest => dest.ProfilePictureUrl, opt => opt.Ignore());

        CreateMap<ChatMessage, SendChatMessageCommand>().ReverseMap();

        CreateMap<ChatMessage, ChatMessageDto>()
            .ForMember(dest => dest.Attachment,
                opt => opt.MapFrom(src => src.Attachments != null
                    ? src.Attachments.Select(a => a.ImageUrl).ToList()
                    : new List<string>()))
            .ForMember(dest => dest.ReactionSummaryDtos, opt => opt.Ignore())
            .AfterMap((src, dest, ctx) =>
            {
                var currentUserId = ctx.Items.ContainsKey("UserId") ? (int)ctx.Items["UserId"] : 0;
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
            .ForMember(dest => dest.CarName, opt => opt.MapFrom(src => $"{src.Auction.Car.Model.Make.Name} {src.Auction.Car.Model.Name}"))
            .ForMember(dest => dest.Engine, opt => opt.MapFrom(src => src.Auction.Car.Engine))
            .ForMember(dest => dest.Drivetrain, opt => opt.MapFrom(src => src.Auction.Car.Drivetrain.ToString()))
            .ForMember(dest => dest.Transmission, opt => opt.MapFrom(src => src.Auction.Car.TransmissionType.ToString()))
            .ForMember(dest => dest.BodyStyle, opt => opt.MapFrom(src => src.Auction.Car.BodyStyle.ToString()))
            .ForMember(dest => dest.ExteriorColor, opt => opt.MapFrom(src => src.Auction.Car.ExteriorColor))
            .ForMember(dest => dest.InteriorColor, opt => opt.MapFrom(src => src.Auction.Car.InteriorColor));

        CreateMap<Comment, UserCommentDto>()
            .ForMember(dest => dest.AuctionId, opt => opt.MapFrom(src => src.AuctionId))
            .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.Auction.CarId))
            .ForMember(dest => dest.CarName, opt => opt.MapFrom(src => $"{src.Auction.Car.Model.Make.Name} {src.Auction.Car.Model.Name}"))
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
    }
}

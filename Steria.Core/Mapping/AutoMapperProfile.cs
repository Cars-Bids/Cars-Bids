using AutoMapper;
using Steria.Core.CQRS.Account;
using Steria.Core.CQRS.Auctions;
using Steria.Core.CQRS.BodyStyles;
using Steria.Core.CQRS.Cars;
using Steria.Core.CQRS.Chat;
using Steria.Core.CQRS.Makes;
using Steria.Core.CQRS.Models;
using Steria.Core.CQRS.Profile;
using Steria.Core.DTOs;
using Steria.Core.Entities;

namespace Steria.Core.Mapping;

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

        CreateMap<Model, ModelDto>().ReverseMap();
        CreateMap<Model, UpdateModelCommand>().ReverseMap();
        CreateMap<Model, CreateModelCommand>().ReverseMap();

        CreateMap<CarImage, CarImageDto>().ReverseMap();

        CreateMap<Car, CarDto>().ReverseMap();
        CreateMap<Car, UpdateCarCommand>().ReverseMap();
        CreateMap<Car, CreateCarCommand>().ReverseMap();

        CreateMap<User, ProfileDto>().ReverseMap();
        CreateMap<User, UpdateProfileCommand>().ReverseMap();
        
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
    }
}

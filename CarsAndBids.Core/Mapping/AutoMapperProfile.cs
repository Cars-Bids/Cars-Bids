using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Entities;
using AutoMapper;
using CarsAndBids.Core.Commands.BodyStyles;
using CarsAndBids.Core.Commands.Makes;
using CarsAndBids.Core.Commands.Models;
using CarsAndBids.Core.Commands.Cars;
using CarsAndBids.Core.Commands.Profile;
using CarsAndBids.Core.Commands.Chat;
using CarsAndBids.Core.Commands.Account;

namespace CarsAndBids.Core.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Auction, AuctionDto>().ReverseMap();

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

        CreateMap<ChatMessage, ChatMessageDto>().ReverseMap();
        CreateMap<ChatMessage, SendChatMessageCommand>().ReverseMap();

        

        CreateMap<RegisterCommand, User>();
    }
}

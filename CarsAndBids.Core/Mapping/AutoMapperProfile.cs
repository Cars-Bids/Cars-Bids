using CarsAndBids.Core.DTOs;
using CarsAndBids.Data.Entities;
using AutoMapper;
using CarsAndBids.Core.CQRS.Account;

namespace CarsAndBids.Core.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Auction, AuctionDto>().ReverseMap();
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<PendingCar, PendingCarDto>().ReverseMap();
        CreateMap<BodyStyle, BodyStyleDto>().ReverseMap();
        CreateMap<Make, MakeDto>().ReverseMap();
        CreateMap<Model, ModelDto>().ReverseMap();
        CreateMap<Car, CarDto>().ReverseMap();
        CreateMap<User, ProfileDto>().ReverseMap();

        CreateMap<RegisterCommand, User>();
    }
}

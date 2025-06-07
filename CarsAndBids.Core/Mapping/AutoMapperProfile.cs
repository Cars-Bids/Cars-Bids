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

        CreateMap<RegisterCommand, User>();
    }
}

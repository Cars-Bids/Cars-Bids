using CarsAndBids.Core.DTOs;
using CarsAndBids.Data.Entities;
using AutoMapper;

namespace CarsAndBids.Core.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Auction, AuctionDto>().ReverseMap();
        CreateMap<Category, CategoryDto>().ReverseMap();
    }
}

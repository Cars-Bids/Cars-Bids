using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Data.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AutoMapper;

namespace CarsAndBids.Core.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Auction, AuctionDto>().ReverseMap();
        }
    }
}

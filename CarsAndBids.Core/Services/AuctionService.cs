using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;

namespace CarsAndBids.Core.Services;

public class AuctionService : IAuctionService
{
    private readonly IGenericRepository<Auction> _auctionRepository;
    private readonly IMapper _mapper;

    public AuctionService(IGenericRepository<Auction> auctionRepository, IMapper mapper)
    {
        _auctionRepository = auctionRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AuctionDto>> GetAllAsync()
    {
        var auctions = await _auctionRepository.GetAsync();
        return _mapper.Map<IEnumerable<AuctionDto>>(auctions);
    }
}

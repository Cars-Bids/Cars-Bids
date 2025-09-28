using AutoMapper;
using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Interfaces;
using Steria.Core.Specification.AuctionSpec;
using Steria.Core.Specification.CarSpec;

namespace Steria.Core.CQRS.Auctions;

public class GetManagingAuctionQuery : IRequest<ManagingAuctionPageDto>
{
    public int AutionId { get; set; }
}

public class GetManagingAuctionQueryHandler(IMapper mapper,
                                            IGenericRepository<Auction> auctionRepository,
                                            IGenericRepository<CarImage> imageRepository) : IRequestHandler<GetManagingAuctionQuery, ManagingAuctionPageDto>
{
    public async Task<ManagingAuctionPageDto> Handle(GetManagingAuctionQuery request, CancellationToken cancellationToken)
    {
        var spec = new AuctionWithCarSpec(request.AutionId);
        var auction = await auctionRepository.GetItemBySpec(spec, cancellationToken);

        var dto = mapper.Map<ManagingAuctionPageDto>(auction);

        var imgSpec = new CarImagesByCardIdFullSpec(auction.CarId);
        var images = await imageRepository.GetListBySpec(imgSpec, cancellationToken);

        dto.CarMainPhotoUrl = images.FirstOrDefault(x => x.ImageCategory == ImageCategory.Main).ImageUrl;

        dto.CarExteriorPhotoUrls =
            mapper.Map<List<CarImageDto>>(images.Where(x => x.ImageCategory == ImageCategory.Exterior));
        
        dto.CarInteriorPhotoUrls =
            mapper.Map<List<CarImageDto>>(images.Where(x => x.ImageCategory == ImageCategory.Interior));
        
        dto.CarOtherPhotoUrls =
            mapper.Map<List<CarImageDto>>(images.Where(x => x.ImageCategory == ImageCategory.Other));

        return dto;
    }
}
using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Exceptions;
using Steria.Core.Interfaces;
using Steria.Core.Resources;
using System.Net;

namespace Steria.Core.CQRS.Auctions;

public class GetAuctionDetailedByIdQuery : IRequest<AuctionDetailedDto>
{
    public int AuctionId { get; set; }
    public int UserId { get; set; }
}

public class GetAuctionDetailedByIdQueryHandler(
    IAuctionRepository repo
    ) : IRequestHandler<GetAuctionDetailedByIdQuery, AuctionDetailedDto>
{
    public async Task<AuctionDetailedDto> Handle(GetAuctionDetailedByIdQuery request, CancellationToken cancellationToken)
    {
        var action = await repo.GetAuctionByIdAsync(request.AuctionId, request.UserId)
            ?? throw new HttpException(string.Format(Resource.AuctionNotFoundById, request.AuctionId), HttpStatusCode.NotFound);

        CarData? car = null;
        List<CommentData> comments = [];
        List<OtherAuction> auctions = [];
        List<CarImageData> images = [];
        List<QAData> qa = [];

        async Task LoadCar() => car = await repo.GetAuctionCarByIdAsync(action.CarId);
        async Task LoadComments() => comments = await repo.GetCommentsByAuctionIdAsync(request.AuctionId);
        async Task LoadImages() => images = await repo.GetCarImagesByCarIdAsync(action.CarId);
        async Task LoadQA() => qa = await repo.GetQaByAuctionIdAsync(request.AuctionId);
        async Task LoadOtherAuctions() => auctions = await repo.GetOtherAuctionsAsync(request.AuctionId, request.UserId);

        await Task.WhenAll(
            LoadCar(),
            LoadComments(),
            LoadImages(),
            LoadQA(),
            LoadOtherAuctions()
        );

        return new AuctionDetailedDto 
        {
            Auction = action,
            Car = car!,
            Comments = comments,
            Auctions = auctions,
            Images = images,
            Qa = qa
        };
    }
}

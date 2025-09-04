using AutoMapper;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Interfaces;
using Steria.Core.Specification.Profile;
using MediatR;

namespace Steria.Core.CQRS.Profile;

public class GetUserInReviewCarsQuery : IRequest<PagedResult<ProfileEndedCarDto>>
{
    public int UserId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public GetUserInReviewCarsQuery(int userId, int pageNumber, int pageSize)
    {
        UserId = userId;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
public class GetUserInReviewCarsHandler(
    IGenericRepository<Car> carRepository,
    IMapper mapper
    ) : IRequestHandler<GetUserInReviewCarsQuery, PagedResult<ProfileEndedCarDto>>
{
    public async Task<PagedResult<ProfileEndedCarDto>> Handle(GetUserInReviewCarsQuery request, CancellationToken cancellationToken)
    {
        var spec = new UserInReviewCarsSpec(request.UserId, request.PageNumber, request.PageSize);
        var cars = await carRepository.GetListBySpec(spec, cancellationToken);

        var ProfileEndedCarDtos = mapper.Map<List<ProfileEndedCarDto>>(cars);

        var totalCount = await carRepository.CountAsync(spec, cancellationToken);

        return new PagedResult<ProfileEndedCarDto>
        {
            Items = ProfileEndedCarDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
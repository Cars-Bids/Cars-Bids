using AutoMapper;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.Profile;
using MediatR;

namespace Steria.Core.CQRS.Profile;

public class GetUserInReviewCarsQuery : IRequest<PagedResult<ProfileInReviewCarDto>>
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
    ) : IRequestHandler<GetUserInReviewCarsQuery, PagedResult<ProfileInReviewCarDto>>
{
    public async Task<PagedResult<ProfileInReviewCarDto>> Handle(GetUserInReviewCarsQuery request, CancellationToken cancellationToken)
    {
        var spec = new UserInReviewCarsSpec(request.UserId, request.PageNumber, request.PageSize);
        var cars = await carRepository.GetListBySpec(spec, cancellationToken);

        var ProfileInReviewCarDtos = mapper.Map<List<ProfileInReviewCarDto>>(cars);

        var countSpec = new UserInReviewCarsCountSpec(request.UserId);
        var totalCount = await carRepository.CountAsync(countSpec, cancellationToken);

        return new PagedResult<ProfileInReviewCarDto>
        {
            Items = ProfileInReviewCarDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Enums;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Core.Specification.Profile;
using MediatR;

namespace CarsAndBids.Core.CQRS.Profile;

public class GetUserInReviewCarsQuery : IRequest<PagedResult<CarDto>>
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
    ) : IRequestHandler<GetUserInReviewCarsQuery, PagedResult<CarDto>>
{
    public async Task<PagedResult<CarDto>> Handle(GetUserInReviewCarsQuery request, CancellationToken cancellationToken)
    {
        var spec = new UserInReviewCarsSpec(request.UserId, request.PageNumber, request.PageSize);
        var cars = await carRepository.GetListBySpec(spec, cancellationToken);

        var carDtos = cars.Select(car =>
        {
            var dto = mapper.Map<CarDto>(car);
            dto.Images = car.Images
                .Where(img => img.ImageCategory == ImageCategory.Main)
                .Select(img => new CarImageDto { ImageUrl = img.ImageUrl, ImageCategory = img.ImageCategory, OrderNumber = img.OrderNumber, UploadedAt = img.UploadedAt })
                .Take(1)
                .ToList();
            return dto;
        }).ToList();

        var totalCount = await carRepository.CountAsync(spec, cancellationToken);

        return new PagedResult<CarDto>
        {
            Items = carDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
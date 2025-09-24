using AutoMapper;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Exceptions;
using Steria.Core.Interfaces;
using Steria.Core.Resources;
using Steria.Core.Specification.CarSpec;
using System.Net;
using MediatR;

namespace Steria.Core.CQRS.Cars;

public class GetCarByIdManagerQuery : IRequest<CarManagerDto>
{
    public int Id { get; set; }
}

public class GetCarByIdManagerHandler(
    IMapper mapper,
    IGenericRepository<Car> carRepository,
    IGenericRepository<CarImage> carImageRepository
    ) : IRequestHandler<GetCarByIdManagerQuery, CarManagerDto>
{
    public async Task<CarManagerDto> Handle(GetCarByIdManagerQuery request, CancellationToken cancellationToken)
    {
        var spec = new CarByIdForManagerSpec(request.Id);
        var car = await carRepository.GetItemBySpec<Car>(spec, cancellationToken)
            ?? throw new HttpException(string.Format(Resource.CarNotFoundById, request.Id), HttpStatusCode.NotFound);

        var dto = mapper.Map<CarManagerDto>(car);
        dto.Images = mapper.Map<List<CarImageDto>>(car.Images) ?? [];

        return dto;
    }
}
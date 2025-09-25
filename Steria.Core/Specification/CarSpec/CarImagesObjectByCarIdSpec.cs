using Ardalis.Specification;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Steria.Core.Entities;

public class CarImagesObjectByCarIdSpec : Specification<CarImage, CarImage>
{
    public CarImagesObjectByCarIdSpec(int carId)
    {
        Query
            .Where(ci => ci.CarId == carId)
            .AsNoTracking()

            .Select(ci => ci);
    }
}
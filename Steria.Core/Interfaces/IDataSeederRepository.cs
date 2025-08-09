using Microsoft.AspNetCore.Identity;

namespace CarsAndBids.Core.Interfaces;
public interface IDataSeederRepository
{
    Task SeedRolesAsync();
    Task SeedBodyStylesAsync();
}
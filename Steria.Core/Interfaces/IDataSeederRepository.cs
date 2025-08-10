using Microsoft.AspNetCore.Identity;

namespace Steria.Core.Interfaces;
public interface IDataSeederRepository
{
    Task SeedRolesAsync();
    Task SeedBodyStylesAsync();
}
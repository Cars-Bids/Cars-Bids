using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsAndBids.Data.Persistence;
using Microsoft.AspNetCore.Identity;

namespace CarsAndBids.Data.Interfaces;
public interface IDataSeederRepository
{
    Task SeedRolesAsync(RoleManager<IdentityRole<int>> roleManager);
    Task SeedBodyStylesAsync(ApplicationDbContext context);
}
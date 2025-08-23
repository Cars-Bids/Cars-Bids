using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Steria.Data.Persistence.Seed;

public class BodyStyleSeed(
    IGenericRepository<BodyStyle> bodyStyleRepository
    )
{
    public async Task SeedAsync()
    {
        // Отримуємо всі існуючі стилі з бази
        var existingStyles = await bodyStyleRepository.GetAsync();

        // Перевіряємо, чи є вже записи в базі
        if (!existingStyles.Any())
        {
            List<BodyStyle> bodyStyles =
            [
                new() { StyleName = "Sedan" },
                new() { StyleName = "Hatchback" },
                new() { StyleName = "Liftback" },
                new() { StyleName = "Fastback" },
                new() { StyleName = "Station Wagon" },
                new() { StyleName = "Coupe" },
                new() { StyleName = "Convertible" },
                new() { StyleName = "Roadster" },
                new() { StyleName = "Targa" },
                new() { StyleName = "Landaulet" },
                new() { StyleName = "SUV" },
                new() { StyleName = "Crossover" },
                new() { StyleName = "Pickup" },
                new() { StyleName = "Double Cab" },
                new() { StyleName = "Extended Cab" },
                new() { StyleName = "Jeep" },
                new() { StyleName = "Minivan" },
                new() { StyleName = "Van" },
                new() { StyleName = "Panel Van" },
                new() { StyleName = "Cargo Van" },
                new() { StyleName = "Passenger Van" },
                new() { StyleName = "Camper Van" },
                new() { StyleName = "Motorhome" },
                new() { StyleName = "Chassis Cab" },
                new() { StyleName = "Box Truck" },
                new() { StyleName = "Luton Van" },
                new() { StyleName = "Flatbed Truck" },
                new() { StyleName = "Tipper" },
                new() { StyleName = "Dump Truck" },
                new() { StyleName = "Tow Truck" },
                new() { StyleName = "Shooting Brake" },
                new() { StyleName = "Phaeton" },
                new() { StyleName = "Hardtop" },
                new() { StyleName = "Microcar" },
                new() { StyleName = "Kei Car" },
                new() { StyleName = "Buggy" },
                new() { StyleName = "Dune Buggy" },
                new() { StyleName = "Limousine" },
                new() { StyleName = "Hearse" },
                new() { StyleName = "Cabrio Coach" },
                new() { StyleName = "Semi-Convertible" }
            ];

            await bodyStyleRepository.InsertRangeAsync(bodyStyles);
        }
    }
}
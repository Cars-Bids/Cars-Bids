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
            var bodyStyles = new List<BodyStyle>
            {
                new BodyStyle { StyleName = "Sedan" },
                new BodyStyle { StyleName = "Hatchback" },
                new BodyStyle { StyleName = "Liftback" },
                new BodyStyle { StyleName = "Fastback" },
                new BodyStyle { StyleName = "Station Wagon" },
                new BodyStyle { StyleName = "Coupe" },
                new BodyStyle { StyleName = "Convertible" },
                new BodyStyle { StyleName = "Roadster" },
                new BodyStyle { StyleName = "Targa" },
                new BodyStyle { StyleName = "Landaulet" },
                new BodyStyle { StyleName = "SUV" },
                new BodyStyle { StyleName = "Crossover" },
                new BodyStyle { StyleName = "Pickup" },
                new BodyStyle { StyleName = "Double Cab" },
                new BodyStyle { StyleName = "Extended Cab" },
                new BodyStyle { StyleName = "Jeep" },
                new BodyStyle { StyleName = "Minivan" },
                new BodyStyle { StyleName = "Van" },
                new BodyStyle { StyleName = "Panel Van" },
                new BodyStyle { StyleName = "Cargo Van" },
                new BodyStyle { StyleName = "Passenger Van" },
                new BodyStyle { StyleName = "Camper Van" },
                new BodyStyle { StyleName = "Motorhome" },
                new BodyStyle { StyleName = "Chassis Cab" },
                new BodyStyle { StyleName = "Box Truck" },
                new BodyStyle { StyleName = "Luton Van" },
                new BodyStyle { StyleName = "Flatbed Truck" },
                new BodyStyle { StyleName = "Tipper" },
                new BodyStyle { StyleName = "Dump Truck" },
                new BodyStyle { StyleName = "Tow Truck" },
                new BodyStyle { StyleName = "Shooting Brake" },
                new BodyStyle { StyleName = "Phaeton" },
                new BodyStyle { StyleName = "Hardtop" },
                new BodyStyle { StyleName = "Microcar" },
                new BodyStyle { StyleName = "Kei Car" },
                new BodyStyle { StyleName = "Buggy" },
                new BodyStyle { StyleName = "Dune Buggy" },
                new BodyStyle { StyleName = "Limousine" },
                new BodyStyle { StyleName = "Hearse" },
                new BodyStyle { StyleName = "Cabrio Coach" },
                new BodyStyle { StyleName = "Semi-Convertible" }
            };

            // Додаємо кожен стиль по одному через InsertAsync
            foreach (var style in bodyStyles)
            {
                await bodyStyleRepository.InsertAsync(style);
            }
        }
    }
}
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.СommonSpec;

namespace Steria.Data.Persistence.Seed;

public class ModelSeeder(IGenericRepository<Make> makeRepository,
                         IGenericRepository<Model> modelRepository)                    
{
    public async Task SeedAsync()
    {
        var existing = await modelRepository.GetItemBySpec(new FirstRecordSpec<Model>());
        if (existing is not null) return;
        
        var makes = (await makeRepository.GetAsync()).ToDictionary(m => m.Name, m => m.Id);

        var makeModels = new Dictionary<string, List<string>>
        {
            { "Toyota", new List<string> { "Corolla", "Camry", "RAV4", "Tacoma", "Highlander", "4Runner", "Prius", "Tundra", "Sienna", "Sequoia", "Land Cruiser", "Supra", "GR86", "Crown", "Venza", "bZ4X", "Mirai", "C-HR", "Avalon", "Yaris" } },
            { "Ford", new List<string> { "F-150", "Mustang", "Explorer", "Escape", "Bronco", "Ranger", "Expedition", "Maverick", "Edge", "Transit", "Focus", "Fiesta", "Fusion", "Taurus", "EcoSport", "GT", "Mach-E", "F-250", "Bronco Sport", "Excursion" } },
            { "Chevrolet", new List<string> { "Silverado", "Equinox", "Tahoe", "Malibu", "Traverse", "Suburban", "Colorado", "Blazer", "Camaro", "Corvette", "TrailBlazer", "Bolt EV", "Spark", "Impala", "Express", "Trax", "Volt", "Cruze", "Sonic", "Avalanche" } },
            { "Honda", new List<string> { "Civic", "Accord", "CR-V", "Pilot", "Odyssey", "HR-V", "Ridgeline", "Passport", "Fit", "Insight", "Clarity", "Prelude", "S2000", "Element", "CR-Z", "Crosstour", "Del Sol", "CRX", "Wagovan", "Acty" } },
            { "Nissan", new List<string> { "Altima", "Sentra", "Rogue", "Pathfinder", "Murano", "Frontier", "Titan", "Armada", "Kicks", "Versa", "Maxima", "GT-R", "Z", "Leaf", "Ariya", "Juke", "Xterra", "Cube", "Quest", "NV200" } },
            { "Hyundai", new List<string> { "Elantra", "Sonata", "Tucson", "Santa Fe", "Palisade", "Kona", "Venue", "Santa Cruz", "Ioniq 5", "Ioniq 6", "Nexo", "Veloster", "Accent", "Azera", "Genesis Coupe", "Tiburon", "Veracruz", "Entourage", "Equus", "Excel" } },
            { "Kia", new List<string> { "Forte", "Optima", "Sorento", "Sportage", "Telluride", "K5", "Soul", "Seltos", "Carnival", "Rio", "Stinger", "EV6", "Niro", "Cadenza", "Sedona", "Amanti", "Borrego", "Rondo", "Spectra", "Sephia" } },
            { "Volkswagen", new List<string> { "Jetta", "Passat", "Tiguan", "Atlas", "Golf", "ID.4", "Taos", "Arteon", "Beetle", "Polo", "Touareg", "CC", "Eos", "Phaeton", "Routan", "Eurovan", "Corrado", "Scirocco", "Vanagon", "Thing" } },
            { "BMW", new List<string> { "3 Series", "5 Series", "X3", "X5", "4 Series", "7 Series", "i4", "iX", "2 Series", "X1", "X7", "M3", "M5", "Z4", "8 Series", "i3", "6 Series", "1 Series", "M4", "X6" } },
            { "Mercedes-Benz", new List<string> { "C-Class", "E-Class", "GLC", "GLE", "S-Class", "GLA", "GLB", "EQE", "EQS", "A-Class", "CLS", "SL", "AMG GT", "G-Class", "Sprinter", "Metris", "SLK", "CLK", "R-Class", "B-Class" } },
            { "Subaru", new List<string> { "Outback", "Forester", "Crosstrek", "Ascent", "Impreza", "Legacy", "WRX", "BRZ", "Solterra", "Baja", "Tribeca", "SVX", "Justy", "XT", "Loyale", "Brat", "Alcyone", "Leone", "Vivio", "Sambar" } },
            { "Mazda", new List<string> { "CX-5", "Mazda3", "CX-30", "CX-50", "Mazda6", "MX-5 Miata", "CX-9", "CX-90", "MX-30", "RX-8", "RX-7", "Tribute", "Protege", "Millenia", "MPV", "B-Series", "626", "929", "Navajo", "323" } },
            { "Tesla", new List<string> { "Model 3", "Model Y", "Model S", "Model X", "Cybertruck", "Roadster", "Semi", "Model 2", "Cyberquad", "Plaid", "Model 3 Performance", "Model Y Long Range", "Model S Plaid", "Model X Plaid", "3 Highland", "Y Juniper", "Robotaxi", "Powerwall", "Solar Roof", "Megapack" } }, // Tesla has fewer, so variants and products, but adjust to models; repeat variants
            { "Audi", new List<string> { "A4", "A6", "Q5", "Q7", "A3", "Q3", "A5", "A8", "e-tron", "Q8", "RS5", "S4", "TT", "R8", "A7", "Q4 e-tron", "S5", "SQ5", "A1", "Q2" } },
            { "Jeep", new List<string> { "Wrangler", "Grand Cherokee", "Cherokee", "Compass", "Renegade", "Gladiator", "Wagoneer", "Grand Wagoneer", "Patriot", "Liberty", "Commander", "CJ-7", "Scrambler", "Comanche", "Willys", "DJ", "FC", "SJ", "XJ", "YJ" } },
            { "Ram", new List<string> { "1500", "2500", "3500", "ProMaster", "ProMaster City", "Dakota", "Power Wagon", "Rebel", "Tradesman", "Laramie", "Big Horn", "Limited", "Classic", "Chassis Cab", "1500 REV", "Rampage", "700", "1200", "1500 TRX", "Warlock" } },
            { "GMC", new List<string> { "Sierra", "Yukon", "Terrain", "Acadia", "Canyon", "Savana", "Hummer EV", "Yukon XL", "Envoy", "Jimmy", "Safari", "Sonoma", "Suburban", "Typhoon", "Syclone", "Vandura", "Rally", " Caballero", "Sprint", "Beauville" } },
            { "Lexus", new List<string> { "RX", "ES", "NX", "GX", "LX", "IS", "UX", "LS", "LC", "RC", "RZ", "TX", "GS", "SC", "HS", "CT", "LFA", "LM", "LY", "LBX" } },
            { "Porsche", new List<string> { "911", "Cayenne", "Panamera", "Macan", "Taycan", "718 Boxster", "718 Cayman", "Carrera GT", "918 Spyder", "959", "944", "928", "914", "356", "Cayenne Coupe", "Panamera Sport Turismo", "Taycan Cross Turismo", "Mission E", "935", "917" } },
            { "Volvo", new List<string> { "XC90", "XC60", "S60", "XC40", "S90", "V60", "V90", "C40", "EX30", "EX90", "240", "740", "850", "940", "960", "S40", "V40", "C30", "C70", "P1800" } }
        };

        var modelsToAdd = new List<Model>();

        foreach (var kv in makeModels)
        {
            if (!makes.TryGetValue(kv.Key, out var makeId)) continue;
            var modelsForMake = kv.Value.Select(name => new Model { Name = name, MakeId = makeId });
            modelsToAdd.AddRange(modelsForMake);
        }

        if (modelsToAdd.Any())
        {
            await modelRepository.InsertRangeAsync(modelsToAdd);
        }
    }
}
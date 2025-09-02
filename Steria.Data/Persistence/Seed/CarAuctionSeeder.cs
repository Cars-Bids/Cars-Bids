using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Interfaces;
using Steria.Core.Specification.ModelSpec;
using Steria.Core.Specification.СommonSpec;

namespace Steria.Data.Persistence.Seed;

public class CarAuctionSeeder(IGenericRepository<Car> carRepository,
                              IGenericRepository<Auction> auctionRepository,
                              IGenericRepository<Model> modelRepository,
                              IGenericRepository<BodyStyle> bodyStyleRepository,
                              UserManager<User> userManager)               
{
    public async Task SeedAsync()
    {
        var existing = await carRepository.GetItemBySpec(new FirstRecordSpec<Car>());

        if (existing is not null) return;
        
        // Fetch existing data
        var allUsers = await userManager.Users.ToListAsync();
        var managers = await userManager.GetUsersInRoleAsync("Manager");
        var allModels = await modelRepository.GetListBySpec(new ModelWithMakeSpec());
        var bodyStyles = (await bodyStyleRepository.GetAsync()).ToList();
        
        if (!allUsers.Any() || !allModels.Any()) return;

        var random = new Random();
        
        var photoLinks = new List<string>
        {
            "https://res.cloudinary.com/carsbids/image/upload/v1756523911/images/c9abee61-0468-4997-9fc0-97ce35b0e856.webp",
            "https://res.cloudinary.com/carsbids/image/upload/v1756523912/images/85e80f5d-ad0d-4dd3-8939-5d65f813bceb.webp",
            "https://res.cloudinary.com/carsbids/image/upload/v1756523912/images/527f0d65-d8e2-4c22-95bf-054b414f5765.webp",
            "https://res.cloudinary.com/carsbids/image/upload/v1756523909/images/a2d94b69-a03d-4732-8958-a36a4c5ef435.jpg",
            "https://res.cloudinary.com/carsbids/image/upload/v1756523909/images/cdd2b6ce-4f99-4f2a-8818-fa65ec1ccf94.webp",
            "https://res.cloudinary.com/carsbids/image/upload/v1756523909/images/c5016f7c-12d5-4abf-bd2d-ca95fc710267.webp",
            "https://res.cloudinary.com/carsbids/image/upload/v1756523909/images/9307b78b-ed60-48e3-9c86-0b480ff48386.webp",
            "https://res.cloudinary.com/carsbids/image/upload/v1756523910/images/22eb1ddb-8ef7-4cfd-ab14-e82af6eefec4.jpg",
            "https://res.cloudinary.com/carsbids/image/upload/v1756523908/images/b50d25e6-e33f-42e6-96f0-02524c190d6a.webp",
            "https://res.cloudinary.com/carsbids/image/upload/v1756523915/images/29070573-9229-4bda-9081-dd7074f7417f.webp",
            "https://res.cloudinary.com/carsbids/image/upload/v1756523915/images/7267cf4d-634d-479e-8b82-02994d645228.webp",
            "https://res.cloudinary.com/carsbids/image/upload/v1756523915/images/ba0c0086-b02d-40af-a4e7-dd6523b425a6.webp",
            "https://res.cloudinary.com/carsbids/image/upload/v1756523913/images/9036c971-e6f8-4803-9d0d-19036b7bcaf1.webp",
            "https://res.cloudinary.com/carsbids/image/upload/v1756523914/images/34a1d03b-1781-4984-9855-14805510cc0a.webp",
            "https://res.cloudinary.com/carsbids/image/upload/v1756523914/images/363f85d6-5f9c-4337-86ac-d0bc3c4ee715.webp",
            "https://res.cloudinary.com/carsbids/image/upload/v1756523913/images/4315e9d8-83b2-47d0-ae1e-e46993faac77.webp",
            "https://res.cloudinary.com/carsbids/image/upload/v1756523913/images/91e1a34f-c74d-4773-a77c-83a4f9511e42.webp",
            "https://res.cloudinary.com/carsbids/image/upload/v1756523910/images/f4df8cfb-71a7-4346-9350-f7d722b11c85.jpg",
            "https://res.cloudinary.com/carsbids/image/upload/v1756523908/images/d1b4c2d3-1c24-4c8d-bcaa-1f6542752f6c.webp",
            "https://res.cloudinary.com/carsbids/image/upload/v1756523908/images/49d5a025-95cf-421d-8c38-5020fe0f87ca.webp",
            "https://res.cloudinary.com/carsbids/image/upload/v1756523908/images/6aec7453-5912-4c06-829f-3db933877157.webp"
        };
        
        // Helper lists
        var colors = new[] { "Red", "Blue", "Black", "White", "Silver", "Green", "Yellow", "Gray" };
        var engines = new[] { "V6 3.0L", "I4 2.0L Turbo", "V8 5.0L", "Electric", "Hybrid 2.5L", "Diesel 3.0L" };
        var locations = new[] { "New York, NY", "Los Angeles, CA", "Chicago, IL", "Houston, TX", "Miami, FL", "Seattle, WA" };
        var drivetrains = Enum.GetValues(typeof(DrivetrainType)).Cast<DrivetrainType>().ToArray();
        var transmissions = Enum.GetValues(typeof(TransmissionType)).Cast<TransmissionType>().ToArray();
        var auctionStatusesEnded = new[] { AuctionStatus.Sold, AuctionStatus.NotSold, AuctionStatus.Cancelled };
        
        // Helper to generate random VIN (17 chars alphanumeric)
        string GenerateVin() => new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 17).Select(s => s[random.Next(s.Length)]).ToArray());

        // Helper to generate random Markdown content
        string GenerateRandomMd(string section)
        {
            var items = new[] { "Feature 1", "Detail 2", "Note 3", "Item 4" };
            var randomItems = items.OrderBy(_ => random.Next()).Take(random.Next(2, 5)).ToList();
            return $"# {section}\nThis is a random {section.ToLower()} description.\n- {string.Join("\n- ", randomItems)}\nAdditional text here.";
        }

        // Helper to add photos to car
        void AddPhotosToCar(Car car, bool isType1, List<string> links, int photoIndex)
        {
            car.Images = new List<CarImage>();
            int numPhotos = isType1 ? random.Next(3, 5) : random.Next(5, 11);
            var categories = isType1 ? new[] { ImageCategory.Other } : Enum.GetValues(typeof(ImageCategory)).Cast<ImageCategory>().ToArray();
            
            if (!isType1)
            {
                car.Images.Add(new CarImage
                {
                    ImageUrl = links[photoIndex % links.Count],
                    ImageCategory = ImageCategory.Main,
                    OrderNumber = 1,
                    UploadedAt = DateTime.UtcNow
                });
                photoIndex++;
                numPhotos--;
            }

            for (int j = 0; j < numPhotos; j++)
            {
                car.Images.Add(new CarImage
                {
                    ImageUrl = links[photoIndex % links.Count],
                    ImageCategory = categories[random.Next(categories.Length)],
                    OrderNumber = j + (isType1 ? 0 : 2),
                    UploadedAt = DateTime.UtcNow
                });
                photoIndex++;
            }
        }
        
        int photoIndex = 0;

        // Type 1: Pending cars (10 cars, no auctions)
        for (int i = 0; i < 10; i++)
        {
            var model = allModels[random.Next(allModels.Count)];
            var car = new Car
            {
                Year = random.Next(1980, 2025),
                Vin = GenerateVin(),
                Mileage = random.Next(1000, 200000),
                SellerNotes = GenerateRandomMd("Seller Notes"),
                IsOnSaleElsewhere = random.Next(2) == 0,
                IsModified = random.Next(2) == 0,
                TransmissionType = transmissions[random.Next(transmissions.Length)],
                Speeds = random.Next(4, 11),
                Status = CarStatus.inPending,
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 30)),
                OwnerId = allUsers[random.Next(allUsers.Count)].Id,
                ModelId = model.Id,
                BodyStyleId = bodyStyles[random.Next(bodyStyles.Count)].Id,
                ChatId = null
            };

            AddPhotosToCar(car, true, photoLinks, photoIndex);
            photoIndex += car.Images.Count;

            await carRepository.InsertAsync(car);
        }

        // Type 2: Auction in progress (10 cars with active auctions)
        for (int i = 0; i < 10; i++)
        {
            var model = allModels[random.Next(allModels.Count)];
            var manager = managers[random.Next(managers.Count)];
            var car = new Car
            {
                Year = random.Next(1980, 2025),
                Vin = GenerateVin(),
                Highlights = GenerateRandomMd("Highlights"),
                ServiceHistory = GenerateRandomMd("Service History"),
                Equipment = GenerateRandomMd("Equipment"),
                Flaws = GenerateRandomMd("Flaws"),
                Modifications = GenerateRandomMd("Modifications"),
                OtherItems = GenerateRandomMd("Other Items"),
                OwnershipHistory = GenerateRandomMd("Ownership History"),
                SellerNotes = GenerateRandomMd("Seller Notes"),
                VideoLinks = "https://www.youtube.com/watch?v=M_0LzA6CVbk, https://www.youtube.com/watch?v=dQw4w9WgXcQ",
                ExteriorColor = colors[random.Next(colors.Length)],
                InteriorColor = colors[random.Next(colors.Length)],
                Mileage = random.Next(1000, 200000),
                Location = locations[random.Next(locations.Length)],
                IsOnSaleElsewhere = random.Next(2) == 0,
                IsModified = random.Next(2) == 0,
                Drivetrain = drivetrains[random.Next(drivetrains.Length)],
                Engine = engines[random.Next(engines.Length)],
                TransmissionType = transmissions[random.Next(transmissions.Length)],
                Speeds = random.Next(4, 11),
                Status = CarStatus.Approved,
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 30)),
                ManagerId = manager.Id,
                OwnerId = allUsers[random.Next(allUsers.Count)].Id,
                ModelId = model.Id,
                BodyStyleId = bodyStyles.Any() ? bodyStyles[random.Next(bodyStyles.Count)].Id : null,
                ChatId = null
            };

            AddPhotosToCar(car, false, photoLinks, photoIndex);
            photoIndex += car.Images.Count;

            await carRepository.InsertAsync(car);
            
            var auction = new Auction
            {
                CarId = car.Id,
                SellerId = car.OwnerId,
                StartPrice = 5000,
                CurrentPrice = random.Next(5000, 100000),
                CurrentBidder = allUsers[random.Next(allUsers.Count)].UserName,
                StartTime = DateTime.UtcNow.AddDays(-random.Next(1, 7)),
                EndTime = DateTime.UtcNow.AddDays(1 + i),
                Status = AuctionStatus.Active,
                CreatedAt = car.CreatedAt,
                ApprovedAt = car.CreatedAt.AddDays(1),
                IsInspected = random.Next(2) == 0
            };

            await auctionRepository.InsertAsync(auction);
        }

        // Type 3: Ended auctions (10 cars with ended auctions)
        for (int i = 0; i < 10; i++)
        {
            var model = allModels[random.Next(allModels.Count)];
            var manager = managers[random.Next(managers.Count)];
            var car = new Car
            {
                Year = random.Next(1980, 2025),
                Vin = GenerateVin(),
                Highlights = GenerateRandomMd("Highlights"),
                ServiceHistory = GenerateRandomMd("Service History"),
                Equipment = GenerateRandomMd("Equipment"),
                Flaws = GenerateRandomMd("Flaws"),
                Modifications = GenerateRandomMd("Modifications"),
                OtherItems = GenerateRandomMd("Other Items"),
                OwnershipHistory = GenerateRandomMd("Ownership History"),
                SellerNotes = GenerateRandomMd("Seller Notes"),
                VideoLinks = "https://www.youtube.com/watch?v=M_0LzA6CVbk, https://www.youtube.com/watch?v=dQw4w9WgXcQ",
                ExteriorColor = colors[random.Next(colors.Length)],
                InteriorColor = colors[random.Next(colors.Length)],
                Mileage = random.Next(1000, 200000),
                Location = locations[random.Next(locations.Length)],
                IsOnSaleElsewhere = random.Next(2) == 0,
                IsModified = random.Next(2) == 0,
                Drivetrain = drivetrains[random.Next(drivetrains.Length)],
                Engine = engines[random.Next(engines.Length)],
                TransmissionType = transmissions[random.Next(transmissions.Length)],
                Speeds = random.Next(4, 11),
                Status = CarStatus.Approved,
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(31, 60)),
                ManagerId = manager.Id,
                OwnerId = allUsers[random.Next(allUsers.Count)].Id,
                ModelId = model.Id,
                BodyStyleId = bodyStyles.Any() ? bodyStyles[random.Next(bodyStyles.Count)].Id : null,
                ChatId = null
            };

            AddPhotosToCar(car, false, photoLinks, photoIndex);
            photoIndex += car.Images.Count;

            await carRepository.InsertAsync(car);

            var auction = new Auction
            {
                CarId = car.Id,
                SellerId = car.OwnerId,
                StartPrice = 5000,
                CurrentPrice = random.Next(5000, 100000),
                CurrentBidder = allUsers[random.Next(allUsers.Count)].UserName,
                StartTime = car.CreatedAt.AddDays(1),
                EndTime = DateTime.UtcNow.AddDays(-random.Next(1, 30)),
                Status = auctionStatusesEnded[random.Next(auctionStatusesEnded.Length)],
                CreatedAt = car.CreatedAt,
                ApprovedAt = car.CreatedAt.AddDays(1),
                IsInspected = random.Next(2) == 0
            };

            await auctionRepository.InsertAsync(auction);
        }
            
    }
}
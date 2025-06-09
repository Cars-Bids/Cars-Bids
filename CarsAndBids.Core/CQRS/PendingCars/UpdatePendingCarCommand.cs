using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CarsAndBids.Core.CQRS.PendingCars;

public class UpdatePendingCarCommand : IRequest<PendingCarDto>
{
    public required PendingCarDto PendingCar { get; set; }
}

public class UpdatePendingCarCommandHandler(
    IGenericRepository<PendingCar> repository,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper
    ) : IRequestHandler<UpdatePendingCarCommand, PendingCarDto>
{
    public async Task<PendingCarDto> Handle(UpdatePendingCarCommand cmd, CancellationToken cancellationToken)
    {
        // Отримуємо HttpContext
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new Exception("HttpContext is null.");
        }

        // Отримуємо об'єкт ClaimsPrincipal (користувача)
        var user = httpContext.User;
        if (user == null)
        {
            throw new Exception("User is null.");
        }

        // Шукаємо клейму з типом NameIdentifier
        var nameIdentifierClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (nameIdentifierClaim == null)
        {
            throw new Exception("Claim 'NameIdentifier' not found.");
        }

        // Отримуємо значення клейми
        var nameIdentifierValue = nameIdentifierClaim.Value;

        // Перетворюємо значення в int
        int ownerId = int.Parse(nameIdentifierValue);

        var existingPendingCar = await repository.GetByIdAsync(cmd.PendingCar.Id);

        if (existingPendingCar?.OwnerId != ownerId)
        {
            throw new UnauthorizedAccessException("You are not the owner of this car.");
        }

        mapper.Map(cmd.PendingCar, existingPendingCar);

        try
        {
            await repository.UpdateAsync(existingPendingCar);
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine("EF ERROR: " + ex.InnerException?.Message);
            throw;
        }

        return mapper.Map<PendingCarDto>(existingPendingCar);
    }
}

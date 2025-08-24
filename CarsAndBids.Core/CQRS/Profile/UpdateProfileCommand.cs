using AutoMapper;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Core.Resources;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CarsAndBids.Core.CQRS.Profile;

public class UpdateProfileCommand : IRequest
{
    public int UserId { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Bio { get; set; }
    public IFormFile? ProfilePicture { get; set; }
}

public class UpdateProfileCommandHandler(
    IGenericRepository<User> repository,
    IMapper mapper,
    IFileService fileService
) : IRequestHandler<UpdateProfileCommand>
{
    public async Task Handle(UpdateProfileCommand cmd, CancellationToken cancellationToken)
    {
        var existingUser = await repository.GetByIdAsync(cmd.UserId)
            ?? throw new KeyNotFoundException(Resource.UserNotFound);

        string? oldProfilePictureUrl = existingUser.ProfilePictureUrl;

        if (cmd.ProfilePicture != null)
        {
            var newImageUrl = await fileService.UploadImageAsync(cmd.ProfilePicture);
            existingUser.ProfilePictureUrl = newImageUrl;

            if (!string.IsNullOrWhiteSpace(oldProfilePictureUrl))
            {
                await fileService.DeleteImageByUrlAsync(oldProfilePictureUrl);
            }
        }

        mapper.Map(cmd, existingUser);
        await repository.UpdateAsync(existingUser);
    }
}
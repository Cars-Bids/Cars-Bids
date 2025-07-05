using CarsAndBids.Core.Interfaces;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.Chat;

public class CreateChatCommand : IRequest<int>
{
    public List<int> ParticipantIds { get; set; }
}

public class CreateChatCommandHandler(IGenericRepository<User> userRepository,
                                      IGenericRepository<Data.Entities.Chat> chatRepository) : IRequestHandler<CreateChatCommand, int>
{
    public async Task<int> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        var participants = await userRepository.GetAsync(u => request.ParticipantIds.Contains(u.Id));

        var chat = new Data.Entities.Chat
        {
            Participants = participants.ToList()
        };

        await chatRepository.InsertAsync(chat);
        return chat.Id;
    }
}
using Domain.Base;
using Domain.Contracts;
using Domain.Entities.Actor;

namespace Application.ActorArea.Command.Friends;

public class DeleteFriendCommandHandler : IRequestHandler<DeleteFriendCommand>
{
    private readonly IContext _context;
    private readonly CurrentUser _currentUser;

    public DeleteFriendCommandHandler(IContext context, CurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task Handle(DeleteFriendCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Set<Friend>()
            .FirstOrDefaultAsync(p => p.Id == request.Id && p.UserId == _currentUser.Id, cancellationToken);
        if (entity is null)
            throw new NotFoundException("Friend");
        _context.Set<Friend>().Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}

public class DeleteFriendCommand : IRequest
{
    public long Id { get; set; }
}
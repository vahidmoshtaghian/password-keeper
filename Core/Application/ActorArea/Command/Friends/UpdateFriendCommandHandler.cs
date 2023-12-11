using Domain.Base;
using Domain.Contracts;
using Domain.Entities.Actor;

namespace Application.ActorArea.Command.Friends;

public class UpdateFriendCommandHandler : IRequestHandler<UpdateFriendCommand>
{
    private readonly IContext _context;
    private readonly CurrentUser _currentUser;

    public UpdateFriendCommandHandler(IContext context, CurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }


    public async Task Handle(UpdateFriendCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Set<Friend>()
            .FirstOrDefaultAsync(p =>
                p.Id == request.Id &&
                p.OwnerId == _currentUser.Id, cancellationToken);
        if (entity is null)
            throw new NotFoundException("Friend");

        entity.FirstName = request.FirstName;
        entity.LastName = request.LastName;
        _context.Set<Friend>().Update(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}

#nullable disable

public class UpdateFriendCommand : IRequest
{
    internal long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public void SetId(long id)
    {
        Id = id;
    }
}

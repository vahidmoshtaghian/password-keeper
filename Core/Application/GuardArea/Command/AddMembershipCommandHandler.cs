using Domain.Base;
using Domain.Contracts;
using Domain.Entities.Actor;
using Domain.Entities.Guard;
using Domain.Exceptions.GuardExceptions;

namespace Application.GuardArea.Command;

public class AddMembershipCommandHandler : IRequestHandler<AddMembershipCommand>
{
    private readonly IContext _context;
    private readonly CurrentUser _currentUser;

    public AddMembershipCommandHandler(IContext context, CurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }


    public async Task Handle(AddMembershipCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Set<Friend>()
            .Where(p => p.Id == request.FriendId)
            .Select(p => p.User)
            .FirstOrDefaultAsync();
        if (user is null)
            throw new NotFoundException("User");

        var exists = await _context.Set<Membership>()
            .AnyAsync(p =>
                p.UserId == user.Id &&
                p.OrganizationId == request.Id, cancellationToken);
        if (exists)
            throw new AlreadyMemberException();

        var isOwner = await _context.Set<Membership>()
            .AnyAsync(p =>
                p.IsOwner &&
                p.UserId == _currentUser.Id &&
                p.OrganizationId == request.Id, cancellationToken);
        if (!isOwner)
            throw new OwnerException();

        var entity = request.MapToDomain(user);
        _context.Set<Membership>().Add(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}

public class AddMembershipCommand : IRequest
{
    public long Id { get; set; }
    public long FriendId { get; set; }

    internal Membership MapToDomain(User user) => new() { User = user, OrganizationId = Id };
}

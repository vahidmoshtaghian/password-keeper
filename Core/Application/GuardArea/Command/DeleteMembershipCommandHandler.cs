using Domain.Base;
using Domain.Contracts;
using Domain.Entities.Actor;
using Domain.Entities.Guard;
using Domain.Exceptions.GuardExceptions;

namespace Application.GuardArea.Command;

public class DeleteMembershipCommandHandler : IRequestHandler<DeleteMembershipCommand>
{
    private readonly IContext _context;
    private readonly CurrentUser _currentUser;

    public DeleteMembershipCommandHandler(CurrentUser currentUser, IContext context)
    {
        _currentUser = currentUser;
        _context = context;
    }

    public async Task Handle(DeleteMembershipCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Set<Friend>()
            .Where(p => p.Id == request.FriendId)
            .Select(p => p.User)
            .FirstOrDefaultAsync();
        if (user is null)
            throw new NotFoundException("User");

        var membership = await _context.Set<Membership>()
            .FirstOrDefaultAsync(p =>
                p.OrganizationId == request.Id &&
                p.UserId == user.Id, cancellationToken);
        if (membership is null)
            throw new NotFoundException("Member");

        var isOwner = await _context.Set<Membership>()
            .AnyAsync(p =>
                p.IsOwner &&
                p.UserId == _currentUser.Id &&
                p.OrganizationId == request.Id, cancellationToken);
        if (!isOwner)
            throw new OwnerException();

        _context.Set<Membership>().Remove(membership);

        await _context.SaveChangesAsync(cancellationToken);
    }
}

public class DeleteMembershipCommand : AddMembershipCommand
{

}


using Domain.Base;
using Domain.Contracts;
using Domain.Entities.Guard;
using Domain.Exceptions.GuardExceptions;

namespace Application.GuardArea.Command;

public class DeleteOrganizationCommandHandler : IRequestHandler<DeleteOrganizationCommand>
{
    private readonly IContext _context;
    private readonly CurrentUser _currentUser;

    public DeleteOrganizationCommandHandler(IContext context, CurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task Handle(DeleteOrganizationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Set<Membership>()
        .Where(p =>
                p.UserId == _currentUser.Id &&
                p.Id == request.Id)
            .Include(p => p.Organization)
            .FirstOrDefaultAsync(cancellationToken);
        if (entity is null)
            throw new NotFoundException("Organization");
        if (!entity.IsOwner)
            throw new OwnerException();

        _context.Set<Organization>()
            .Remove(entity.Organization);

        await _context.SaveChangesAsync(cancellationToken);
    }
}

public class DeleteOrganizationCommand : IRequest
{
    public long Id { get; set; }
}

using Domain.Base;
using Domain.Contracts;
using Domain.Entities.Guard;
using Domain.Exceptions.GuardExceptions;

namespace Application.GuardArea.Command;

public class UpdateOrganizationCommandHandler : IRequestHandler<UpdateOrganizationCommand>
{
    private readonly IContext _context;
    private readonly CurrentUser _currentUser;

    public UpdateOrganizationCommandHandler(IContext context, CurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task Handle(UpdateOrganizationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Set<Membership>()
            .Where(p =>
                p.UserId == _currentUser.Id &&
                p.OrganizationId == request.Id)
            .Include(p => p.Organization)
            .FirstOrDefaultAsync(cancellationToken);
        if (entity is null)
            throw new NotFoundException("Organization");
        if (!entity.IsOwner)
            throw new OwnerException();

        entity.Organization.Title = request.Title;
        _context.Set<Organization>().Update(entity.Organization);

        await _context.SaveChangesAsync(cancellationToken);
    }
}

#nullable disable

public class UpdateOrganizationCommand : IRequest
{
    public long Id { get; private set; }
    public string Title { get; set; }

    public void SetId(long id)
    {
        Id = id;
    }
}

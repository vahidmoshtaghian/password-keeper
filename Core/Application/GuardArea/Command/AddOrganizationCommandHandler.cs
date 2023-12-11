using Application.GuardArea.Query;
using Domain.Base;
using Domain.Contracts;
using Domain.Entities.Guard;
using Domain.Exceptions.GuardExceptions;

namespace Application.GuardArea.Command;

public class AddOrganizationCommandHandler : IRequestHandler<AddOrganizationCommand, AddOrganizationCommandResponse>
{
    private readonly IContext _context;
    private readonly CurrentUser _currentUser;

    public AddOrganizationCommandHandler(IContext context, CurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<AddOrganizationCommandResponse> Handle(AddOrganizationCommand request, CancellationToken cancellationToken)
    {
        var duplicate = await _context.Set<Membership>()
            .AnyAsync(p =>
            p.UserId == _currentUser.Id &&
            p.IsOwner == true &&
            p.Organization.Title == request.Title);
        if (duplicate)
            throw new DuplicateEntityException("Organization");

        var entity = request.MapToDomain(_currentUser.Id);
        _context.Set<Organization>().Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return new(entity);
    }
}

public class AddOrganizationCommand : IRequest<AddOrganizationCommandResponse>
{
    public string Title { get; set; }

    internal Organization MapToDomain(long ownerId)
    {
        return new()
        {
            Title = Title,
            Memberships = new List<Membership>
            {
                new()
                {
                    IsOwner = true,
                    UserId = ownerId,
                }
            }
        };
    }
}

public class AddOrganizationCommandResponse : GetAllOrganizationsQueryResponse
{
    public AddOrganizationCommandResponse(Organization entity) : base(entity)
    {
        IsOwner = true;
    }
}

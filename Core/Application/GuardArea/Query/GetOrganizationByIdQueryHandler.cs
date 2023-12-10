using Domain.Base;
using Domain.Contracts;
using Domain.Entities.Guard;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.GuardArea.Query;

public class GetOrganizationByIdQueryHandler : IRequestHandler<GetOrganizationByIdQuery, GetOrganizationByIdQueryResponse>
{
    private readonly IContext _context;
    private readonly CurrentUser _currentUser;

    public GetOrganizationByIdQueryHandler(IContext context, CurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<GetOrganizationByIdQueryResponse> Handle(GetOrganizationByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Set<Membership>()
            .Where(p =>
                p.OrganizationId == request.Id &&
                p.UserId == _currentUser.Id)
            .Include(p => p.Organization)
            .FirstOrDefaultAsync(cancellationToken);
        if (entity is null)
            throw new NotFoundException("Organization");

        return new(entity);
    }
}

public class GetOrganizationByIdQuery : IRequest<GetOrganizationByIdQueryResponse>
{
    public long Id { get; set; }
}

public class GetOrganizationByIdQueryResponse : GetAllOrganizationsQueryResponse
{
    public GetOrganizationByIdQueryResponse(Membership entity) : base(entity)
    {
    }
}

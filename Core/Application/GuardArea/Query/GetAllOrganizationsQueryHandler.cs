using Domain.Base;
using Domain.Base.PaginageDto;
using Domain.Contracts;
using Domain.Entities.Guard;
using Domain.Extensions;

namespace Application.GuardArea.Query;

public class GetAllOrganizationsQueryHandler : IRequestHandler<GetAllOrganizationsQuery, PaginateList<GetAllOrganizationsQueryResponse>>
{
    private readonly IContext _context;
    private readonly CurrentUser _currentUser;

    public GetAllOrganizationsQueryHandler(IContext context, CurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<PaginateList<GetAllOrganizationsQueryResponse>> Handle(GetAllOrganizationsQuery request, CancellationToken cancellationToken)
    {
        var (entities, count) = await _context.Set<Membership>()
            .Where(p =>
                p.UserId == _currentUser.Id &&
                (string.IsNullOrEmpty(request.Search) || p.Organization.Title.Contains(request.Search)))
            .Include(p => p.Organization)
            .OrderBy(p => p.Organization.Title)
            .ToListPaginateAsync(request.Page, request.PageSize);

        var result = entities.Select(p => new GetAllOrganizationsQueryResponse(p));

        return new(result, count, request.PageSize);
    }
}

#nullable disable

public class GetAllOrganizationsQuery : PaginateQuery, IRequest<PaginateList<GetAllOrganizationsQueryResponse>>
{

}

public class GetAllOrganizationsQueryResponse
{
    public GetAllOrganizationsQueryResponse()
    {

    }

    public GetAllOrganizationsQueryResponse(Membership entity)
    {
        Id = entity.OrganizationId;
        IsOwner = entity.IsOwner;
        Title = entity.Organization?.Title;
    }

    public GetAllOrganizationsQueryResponse(Organization entity)
    {
        Id = entity.Id;
        Title = entity.Title;
    }

    public long Id { get; set; }
    public bool IsOwner { get; set; }
    public string Title { get; set; }
}
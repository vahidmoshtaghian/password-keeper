using Application.ActorArea.Query.Friends;
using Domain.Base;
using Domain.Contracts;
using Domain.Entities.Actor;
using Domain.Entities.Guard;

namespace Application.GuardArea.Query;

public class GetOrganizationMembershipsQueryHandler : IRequestHandler<GetOrganizationMembershipsQuery, IEnumerable<GetOrganizationMembershipQueryResponse>>
{
    private readonly IContext _context;
    private readonly CurrentUser _currentUser;

    public GetOrganizationMembershipsQueryHandler(CurrentUser currentUser, IContext context)
    {
        _currentUser = currentUser;
        _context = context;
    }

    public async Task<IEnumerable<GetOrganizationMembershipQueryResponse>> Handle(GetOrganizationMembershipsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _context.Set<Membership>()
            .Where(p => p.OrganizationId == request.Id)
            .Include(p => p.User.Friends.Where(x => x.OwnerId == _currentUser.Id))
            .ThenInclude(p => p.User)
            .ToListAsync(cancellationToken);
        // maybe friend is null here! 
        //var result = entities.Select(p => new GetOrganizationMembershipQueryResponse(p));

        return null;
    }
}

public class GetOrganizationMembershipsQuery : IRequest<IEnumerable<GetOrganizationMembershipQueryResponse>>
{
    public long Id { get; set; }
}

public class GetOrganizationMembershipQueryResponse : GetAllFriendsQueryResponse
{
    public GetOrganizationMembershipQueryResponse(Friend entity) : base(entity)
    {
    }
}
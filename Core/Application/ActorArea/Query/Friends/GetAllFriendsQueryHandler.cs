using Domain.Base;
using Domain.Base.PaginageDto;
using Domain.Contracts;
using Domain.Entities.Actor;
using Domain.Extensions;

namespace Application.ActorArea.Query.Friends;

public class GetAllFriendsQueryHandler : IRequestHandler<GetAllFriendsQuery, PaginateList<GetAllFriendsQueryResponse>>
{
    private readonly IContext _context;
    private readonly CurrentUser _currentUser;

    public GetAllFriendsQueryHandler(IContext context, CurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<PaginateList<GetAllFriendsQueryResponse>> Handle(GetAllFriendsQuery request, CancellationToken cancellationToken)
    {
        var (entities, count) = await _context.Set<Friend>()
            .Where(p =>
                p.UserId == _currentUser.Id &&
                (string.IsNullOrEmpty(request.Search) ||
                p.FirstName.Contains(request.Search) ||
                p.LastName.Contains(request.Search)))
            .OrderBy(p => p.FirstName)
            .ThenBy(p => p.LastName)
            .ToListPaginateAsync(request.Page, request.PageSize);
        var result = entities.Select(p => new GetAllFriendsQueryResponse(p));

        return new(result, count, request.PageSize);
    }
}

public class GetAllFriendsQuery : PaginateQuery, IRequest<PaginateList<GetAllFriendsQueryResponse>>
{

}

public class GetAllFriendsQueryResponse
{
    public GetAllFriendsQueryResponse(Friend entity)
    {
        Id = entity.Id;
        FullName = entity.FullName;
        Phone = entity.User.Phone;
    }

    public long Id { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
}

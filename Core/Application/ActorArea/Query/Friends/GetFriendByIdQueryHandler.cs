using Domain.Base;
using Domain.Contracts;
using Domain.Entities.Actor;

namespace Application.ActorArea.Query.Friends;

public class GetFriendByIdQueryHandler : IRequestHandler<GetFriendByIdQuery, GetFriendByIdQueryResponse>
{
    private readonly IContext _context;
    private readonly CurrentUser _currentUser;

    public GetFriendByIdQueryHandler(IContext context, CurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<GetFriendByIdQueryResponse> Handle(GetFriendByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Set<Friend>()
            .Include(p => p.User)
            .FirstOrDefaultAsync(p =>
                p.Id == request.Id &&
                p.OwnerId == _currentUser.Id, cancellationToken);
        if (entity is null)
            throw new NotFoundException("Friend");
        var result = new GetFriendByIdQueryResponse(entity);

        return result;
    }
}

#nullable disable

public class GetFriendByIdQuery : IRequest<GetFriendByIdQueryResponse>
{
    public long Id { get; set; }
}

public class GetFriendByIdQueryResponse
{
    public GetFriendByIdQueryResponse(Friend entity)
    {
        Id = entity.Id;
        FirstName = entity.FirstName;
        LastName = entity.LastName;
        Phone = entity.User?.Phone;
    }

    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
}
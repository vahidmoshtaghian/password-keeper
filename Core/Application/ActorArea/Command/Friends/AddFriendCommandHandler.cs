using Application.ActorArea.Query.Friends;
using Domain.Base;
using Domain.Contracts;
using Domain.Entities.Actor;
using Domain.Enums;
using System.Net;

namespace Application.ActorArea.Command.Friends;

public class AddFriendCommandHandler : IRequestHandler<AddFriendCommand, AddFriendCommandResponse>
{
    private readonly IContext _context;
    private readonly CurrentUser _currentUser;

    public AddFriendCommandHandler(IContext context, CurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<AddFriendCommandResponse> Handle(AddFriendCommand request, CancellationToken cancellationToken)
    {
        var isYourself = await _context.Set<User>()
            .AnyAsync(p => p.Id == _currentUser.Id && p.Phone == request.Phone);
        if (isYourself)
            throw new CustomException("This is you!", HttpStatusCode.Conflict);

        var exists = await _context.Set<Friend>()
            .AnyAsync(p => p.User.Phone == request.Phone && p.UserId == _currentUser.Id);
        if (exists)
            throw new DuplicateException("friend");

        var user = await _context.Set<User>()
            .FirstOrDefaultAsync(p => p.Phone == request.Phone, cancellationToken);
        user ??= request.MapToUser();

        var entity = request.MapToFriend(user);
        entity.OwnerId = _currentUser.Id;

        _context.Set<Friend>().Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return new AddFriendCommandResponse(entity);
    }
}

#nullable disable

public class AddFriendCommand : IRequest<AddFriendCommandResponse>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }

    internal User MapToUser()
    {
        return new()
        {
            FirstName = FirstName,
            LastName = LastName,
            Phone = Phone,
            Status = UserStatus.UnRegistered
        };
    }

    internal Friend MapToFriend(User user = null)
    {
        return new()
        {
            FirstName = FirstName,
            LastName = LastName,
            User = user
        };
    }
}

public class AddFriendCommandResponse : GetAllFriendsQueryResponse
{
    public AddFriendCommandResponse(Friend entity) : base(entity)
    {
    }
}

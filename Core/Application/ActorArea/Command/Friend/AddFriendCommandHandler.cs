using Domain.Base;
using Domain.Contracts;

namespace Application.ActorArea.Command.Friend;

public class AddFriendCommandHandler : IRequestHandler<AddFriendCommand, AddFriendCommandResponse>
{
    private readonly IContext _context;
    private readonly CurrentUser _currentUser;

    public AddFriendCommandHandler(IContext context, CurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public Task<AddFriendCommandResponse> Handle(AddFriendCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

#nullable disable

public class AddFriendCommand : IRequest<AddFriendCommandResponse>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
}

public class AddFriendCommandResponse
{
    public long Id { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
}

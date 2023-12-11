using Domain.Base;
using Domain.Contracts;
using Domain.Entities.Actor;
using Domain.Exceptions.UserExceptions;

namespace Application.ActorArea.Command.Account;

public class RefreshCommandHandler : IRequestHandler<RefreshCommand, RefreshCommandResponse>
{
    private readonly IContext _context;
    private readonly IIdentityService _identityService;
    private readonly CurrentUser _currentUser;

    public RefreshCommandHandler(IContext context, IIdentityService identityService, CurrentUser currentUser)
    {
        _context = context;
        _identityService = identityService;
        _currentUser = currentUser;
    }

    public async Task<RefreshCommandResponse> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Set<User>()
            .FirstOrDefaultAsync(p =>
                p.Id == _currentUser.Id &&
                p.Status == Domain.Enums.UserStatus.Normal &&
                p.RefreshToken == request.RefreshToken &&
                p.RefreshTokenExpire > DateTime.Now,
                cancellationToken);
        if (entity is null)
            throw new UserNotFoundException();

        entity.SetRefreshToken(_identityService.CreateRefreshToken());
        var token = _identityService.CreateToken(entity);
        _context.Set<User>().Update(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return new() { Token = token, Refresh = entity.RefreshToken };
    }
}

public class RefreshCommand : IRequest<RefreshCommandResponse>
{
    public string RefreshToken { get; set; }
}

public class RefreshCommandResponse : SignInCommandResponse
{

}

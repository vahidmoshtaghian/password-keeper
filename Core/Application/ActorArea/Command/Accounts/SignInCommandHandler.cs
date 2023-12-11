using Domain.Contracts;
using Domain.Entities.Actor;
using Domain.Enums;
using Domain.Exceptions.UserExceptions;

namespace Application.ActorArea.Command.Account;

public class SignInCommandHandler : IRequestHandler<SignInCommand, SignInCommandResponse>
{
    private readonly IContext _context;
    private readonly IIdentityService _identityService;

    public SignInCommandHandler(IIdentityService identityService, IContext context)
    {
        _identityService = identityService;
        _context = context;
    }

    public async Task<SignInCommandResponse> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Set<User>()
            .FirstOrDefaultAsync(p =>
            p.Phone == request.Phone && p.Status == UserStatus.Normal, cancellationToken);
        if (entity is null)
            throw new UserNotFoundException();
        if (!entity.CheckPassword(request.Password))
            throw new UserNotFoundException();

        entity.SetRefreshToken(_identityService.CreateRefreshToken());
        var token = _identityService.CreateToken(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return new() { Token = token, Refresh = entity.RefreshToken };
    }
}

#nullable disable

public class SignInCommand : IRequest<SignInCommandResponse>
{
    public string Phone { get; set; }
    public string Password { get; set; }
}

public class SignInCommandResponse
{
    public string Token { get; internal set; }
    public string Refresh { get; internal set; }
}

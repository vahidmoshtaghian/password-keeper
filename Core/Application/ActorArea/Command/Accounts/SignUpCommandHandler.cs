using Domain.Contracts;
using Domain.Entities.Actor;

namespace Application.ActorArea.Command.Account;

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, SignUpCommandResponse>
{
    private readonly IContext _context;
    private readonly IIdentityService _identityService;

    public SignUpCommandHandler(IContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<SignUpCommandResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var emailExists = await _context.Set<User>()
            .AnyAsync(p => p.Phone == request.Phone, cancellationToken);

        var entity = request.MapToDomain();
        entity.SetRefreshToken(_identityService.CreateRefreshToken());
        _context.Set<User>().Add(entity);
        var token = _identityService.CreateToken(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return new() { Token = token, Refresh = entity.RefreshToken };
    }
}

public class SignUpCommand : IRequest<SignUpCommandResponse>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }

    internal User MapToDomain()
    {
        User user = new()
        {
            FirstName = FirstName,
            LastName = LastName,
            Phone = Phone
        };
        user.SetPassword(Password);
        user.GenerateVerificationCode();

        return user;
    }
}

public class SignUpCommandResponse : SignInCommandResponse
{
}

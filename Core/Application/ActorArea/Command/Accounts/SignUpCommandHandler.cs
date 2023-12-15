using Domain.Contracts;
using Domain.Entities.Actor;
using Domain.Enums;

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
        var entity = await _context.Set<User>()
            .FirstOrDefaultAsync(p => p.Phone == request.Phone, cancellationToken);

        entity = request.MapToDomain(entity);
        entity.SetRefreshToken(_identityService.CreateRefreshToken());

        if (entity.Id == 0)
            _context.Set<User>().Add(entity);
        else
            _context.Set<User>().Update(entity);

        await _context.SaveChangesAsync(cancellationToken);

        var token = _identityService.CreateToken(entity);

        return new() { Token = token, Refresh = entity.RefreshToken };
    }
}

#nullable disable

public class SignUpCommand : IRequest<SignUpCommandResponse>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }

    internal User MapToDomain(User user)
    {
        user ??= new()
        {
            FirstName = FirstName,
            LastName = LastName,
            Phone = Phone
        };
        user.Status = UserStatus.Normal;
        user.SetPassword(Password);
        user.GenerateVerificationCode();

        return user;
    }
}

public class SignUpCommandResponse : SignInCommandResponse
{
}

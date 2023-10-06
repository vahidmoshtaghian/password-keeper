using Domain.Entities.Actor;

namespace Domain.Contracts;

public interface IIdentityService
{
    string CreateRefreshToken();
    string CreateToken();
    string CreateToken(User user);
}

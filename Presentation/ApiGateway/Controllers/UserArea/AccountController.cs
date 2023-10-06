using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers.UserArea;

public class AccountController : ControllerBase
{
    [HttpPost("sign-up")]
    public async Task SignUp()
    {
        throw new NotImplementedException();
    }

    [AllowAnonymous]
    [HttpPost("sign-in")]
    public async Task SignIn()
    {
        throw new NotImplementedException();
    }
}

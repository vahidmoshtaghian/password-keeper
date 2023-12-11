using ApiGateway.Controllers.Base;
using ApiGateway.Extensions;
using Application.ActorArea.Command.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers.UserArea;

[ApiExplorerSettings(GroupName = SwaggerDefinition.UserArea)]
public class AccountController : ApplicationController
{
    [AllowAnonymous]
    [HttpPost("sign-up")]
    public async Task<SignUpCommandResponse> SignUp([FromBody] SignUpCommand command)
    {
        var result = await Mediator.Send(command);

        return result;
    }

    [AllowAnonymous]
    [HttpPost("sign-in")]
    public async Task<SignInCommandResponse> SignIn([FromBody] SignInCommand command)
    {
        var result = await Mediator.Send(command);

        return result;
    }

    [HttpPost("refresh")]
    public async Task<RefreshCommandResponse> Refresh([FromBody] RefreshCommand command)
    {
        var result = await Mediator.Send(command);

        return result;
    }
}

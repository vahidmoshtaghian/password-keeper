using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers.Base
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ApplicationController : ControllerBase
    {
        public IMediator Mediator => HttpContext.RequestServices.GetRequiredService<IMediator>();
    }
}

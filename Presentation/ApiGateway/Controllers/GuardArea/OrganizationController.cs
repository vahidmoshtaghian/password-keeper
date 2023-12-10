using ApiGateway.Controllers.Base;
using ApiGateway.Extensions;
using Application.GuardArea.Command;
using Application.GuardArea.Query;
using Domain.Base.PaginageDto;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers.GuardArea;

[ApiExplorerSettings(GroupName = SwaggerDefinition.GuardArea)]
public class OrganizationController : ApplicationController
{
    [HttpGet]
    public async Task<PaginateList<GetAllOrganizationsQueryResponse>> Get([FromQuery] GetAllOrganizationsQuery query)
    {
        var result = await Mediator.Send(query);

        return result;
    }

    [HttpPost]
    public async Task<AddOrganizationCommandResponse> Add([FromBody] AddOrganizationCommand command)
    {
        var result = await Mediator.Send(command);

        return result;
    }
}

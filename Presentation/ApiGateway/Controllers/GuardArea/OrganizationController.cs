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

    [HttpGet("{id:long}")]
    public async Task<GetOrganizationByIdQueryResponse> GetById([FromRoute] long id)
    {
        var result = await Mediator.Send(new GetOrganizationByIdQuery() { Id = id });

        return result;
    }

    [HttpPost]
    public async Task<AddOrganizationCommandResponse> Add([FromBody] AddOrganizationCommand command)
    {
        var result = await Mediator.Send(command);

        return result;
    }

    [HttpPut("{id:long}")]
    public async Task Update([FromRoute] long id, [FromBody] UpdateOrganizationCommand command)
    {
        command.SetId(id);

        await Mediator.Send(command);
    }

    [HttpDelete("{id:long}")]
    public async Task Delete([FromRoute] long id)
    {
        await Mediator.Send(new DeleteOrganizationCommand() { Id = id });
    }
}

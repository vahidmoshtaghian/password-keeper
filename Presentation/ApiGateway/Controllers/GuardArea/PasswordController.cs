using ApiGateway.Controllers.Base;
using ApiGateway.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers.GuardArea;

[ApiExplorerSettings(GroupName = SwaggerDefinition.GuardArea)]
public class PasswordController : ApplicationController
{
    [HttpGet("{id:long}")]
    public async Task GetById([FromRoute] long id)
    {

    }

    [HttpDelete("{id:long}")]
    public async Task Delete([FromRoute] long id)
    {

    }

    [HttpPatch("{id:long}")]
    public async Task Update([FromRoute] long id, [FromBody] object command)
    {

    }

    [HttpGet("organization/{organizationId:long}")]
    public async Task<List<object>> Get([FromRoute] long organizationId)
    {

    }

    [HttpPost("organization/{organizationId:long}")]
    public async Task Add([FromRoute] long organizationId, [FromBody] object command)
    {

    }


}

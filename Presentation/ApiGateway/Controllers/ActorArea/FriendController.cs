using ApiGateway.Controllers.Base;
using ApiGateway.Extensions;
using Application.ActorArea.Command.Friends;
using Application.ActorArea.Query.Friends;
using Domain.Base.PaginageDto;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers.ActorArea;

[ApiExplorerSettings(GroupName = SwaggerDefinition.UserArea)]
public class FriendController : ApplicationController
{
    [HttpGet]
    public async Task<PaginateList<GetAllFriendsQueryResponse>> Get([FromQuery] GetAllFriendsQuery query)
    {
        var result = await Mediator.Send(query);

        return result;
    }

    [HttpGet("{id:long}")]
    public async Task<GetFriendByIdQueryResponse> Get([FromRoute] long id)
    {
        var result = await Mediator.Send(new GetFriendByIdQuery() { Id = id });

        return result;
    }

    [HttpPost]
    public async Task<AddFriendCommandResponse> GetById([FromBody] AddFriendCommand command)
    {
        var result = await Mediator.Send(command);

        return result;
    }

    [HttpPut("{id:long}")]
    public async Task Update([FromRoute] long id, [FromBody] UpdateFriendCommand command)
    {
        command.SetId(id);

        await Mediator.Send(command);
    }

    [HttpDelete("{id:long}")]
    public async Task Delete([FromRoute] long id)
    {
        await Mediator.Send(new DeleteFriendCommand() { Id = id });
    }
}

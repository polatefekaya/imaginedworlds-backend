using System;
using ImaginedWorlds.Application.Contracts.Creation;
using ImaginedWorlds.Application.Creation.CancelCreation;
using ImaginedWorlds.Application.Creation.StartCreation;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace ImaginedWorlds.Endpoints;

public static class CreationEndpoints
{
    public static void MapCreationEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/creation").WithTags("Creation");

        group.MapPost("/start", async (
            [FromBody] StartCreationRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            StartCreationCommand command = new(
                request.UserPrompt,
                request.AgentCodeName,
                request.ConnectionId
            );

            Ulid creationId = await mediator.Send(command, cancellationToken);
            return Results.Accepted($"/api/creation/status/{creationId}", new { CreationId = creationId.ToString() });
        })
        .WithName("StartCreation")
        .Produces<object>(StatusCodes.Status202Accepted)
        .Produces(StatusCodes.Status400BadRequest);

        group.MapPost("/cancel", async (
            [FromBody] CancelCreationRequest request,
            IMediator mediator) =>
        {
            await mediator.Send(new CancelCreationCommand(request.ConnectionId));
            return Results.Accepted();
        })
        .WithName("CancelCreation");
    }
}

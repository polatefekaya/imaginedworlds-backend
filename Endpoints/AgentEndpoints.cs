using System;
using ImaginedWorlds.Application.AiAgent.AddAgent;
using ImaginedWorlds.Application.AiAgent.GetAllAgents;
using ImaginedWorlds.Application.AiAgent.RemoveAgent;
using ImaginedWorlds.Application.AiAgent.UpdateAgentDetails;
using ImaginedWorlds.Application.Contracts.Agent;
using ImaginedWorlds.Domain.Agent;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace ImaginedWorlds.Endpoints;

public static class AgentEndpoints
{
    public static void MapAgentEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/agents").WithTags("Agents");

        group.MapPost("/", async (
            [FromBody] AddAgentRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            AddAgentCommand command = new(
                request.DisplayName,
                request.CodeName,
                request.Description,
                request.IconUrl,
                request.ProviderConfiguration
            );

            Ulid newAgentId = await mediator.Send(command, cancellationToken);
            return Results.Created($"/api/agents/{request.CodeName}", new { Id = newAgentId.ToString() });
        })
        .WithName("CreateAgent")
        .Produces<object>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);


        group.MapPut("/{codeName}", async (
            [FromRoute] string codeName,
            [FromBody] UpdateAgentDetailsRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            UpdateAgentDetailsParameters parameters = new()
            {
                DisplayName = request.DisplayName,
                Description = request.Description,
                IconUrl = request.IconUrl,
                ProviderConfiguration = request.ProviderConfiguration
            };

            UpdateAgentDetailsCommand command = new(codeName, parameters);

            await mediator.Send(command, cancellationToken);
            return Results.NoContent();
        })
        .WithName("UpdateAgent")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);


        group.MapDelete("/{codeName}", async (
            [FromRoute] string codeName,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            RemoveAgentCommand command = new(codeName);
            await mediator.Send(command, cancellationToken);

            return Results.NoContent();
        })
        .WithName("RemoveAgent")
        .Produces(StatusCodes.Status204NoContent);
        
        group.MapGet("/", async (IMediator mediator, CancellationToken cancellationToken) =>
        {
            GetAllAgentsQuery query = new();
            IReadOnlyList<GetAgentResponse> agents = await mediator.Send(query, cancellationToken);
            
            return Results.Ok(agents);
        })
        .WithName("GetAllAgents")
        .Produces<IReadOnlyList<GetAgentResponse>>(StatusCodes.Status200OK);
    }
}
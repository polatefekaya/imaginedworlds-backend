using ImaginedWorlds.Domain.Common;
using Mediator;

namespace ImaginedWorlds.Application.Grid.CreateGrid;

public record struct CreateGridCommand(
    

) : ICommand<Ulid>;

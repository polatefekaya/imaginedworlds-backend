using Mediator;

namespace ImaginedWorlds.Application.Creation.CancelCreation;

public sealed record CancelCreationCommand(string ConnectionId) : ICommand;
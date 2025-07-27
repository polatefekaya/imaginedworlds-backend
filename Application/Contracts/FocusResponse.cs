namespace ImaginedWorlds.Application.Contracts;

public record FocusResponse(
    int X,
    int Y,
    int Range,
    string Comments
);

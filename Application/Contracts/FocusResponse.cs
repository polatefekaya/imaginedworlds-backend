namespace ImaginedWorlds.Application.Contracts;

public record FocusResponse(
    int X,
    int Y,
    int Range,
    string Comments
)
{
    public override string ToString()
    {
        return $"x: {X}, y: {Y}, range: {Range}, comments: {Comments}";
    }
};

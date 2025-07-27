using System;
using ImaginedWorlds.Application.Contracts;
using ImaginedWorlds.Domain.Common;

namespace ImaginedWorlds.Domain.Grid;

public static class GridHelper
{
    public static byte[,] GetFocusedView(byte[,] mainGrid, FocusResponse focus)
    {
        ArgumentNullException.ThrowIfNull(mainGrid);
        ArgumentNullException.ThrowIfNull(focus);

        int gridWidth = mainGrid.GetLength(0);
        int gridHeight = mainGrid.GetLength(1);

        if (focus.X < 0 || focus.X >= gridWidth || focus.Y < 0 || focus.Y >= gridHeight)
        {
            throw new ArgumentOutOfRangeException(nameof(focus), $"Focus point ({focus.X}, {focus.Y}) is outside the grid boundaries (0-{gridWidth - 1}, 0-{gridHeight - 1}).");
        }

        int halfRange = focus.Range / 2;
        int idealStartX = focus.X - halfRange;
        int idealStartY = focus.Y - halfRange;
        int idealEndX = idealStartX + focus.Range;
        int idealEndY = idealStartY + focus.Range;

        int actualStartX = Math.Max(0, idealStartX);
        int actualStartY = Math.Max(0, idealStartY);
        int actualEndX = Math.Min(gridWidth, idealEndX);
        int actualEndY = Math.Min(gridHeight, idealEndY);

        int viewWidth = actualEndX - actualStartX;
        int viewHeight = actualEndY - actualStartY;

        if (viewWidth <= 0 || viewHeight <= 0)
        {
            return new byte[0, 0];
        }

        byte[,] focusedView = new byte[viewWidth, viewHeight];

        for (int y = 0; y < viewHeight; y++)
        {
            for (int x = 0; x < viewWidth; x++)
            {
                focusedView[x, y] = mainGrid[actualStartX + x, actualStartY + y];
            }
        }

        return focusedView;
    }
    

    public static void CheckBounds(int Height, int Width, Coordinates coordinates)
    {
        bool xInBounds = coordinates.X >= 0 && coordinates.X <= Width;
        bool yInBounds = coordinates.Y >= 0 && coordinates.Y <= Height;

        if (!xInBounds || !yInBounds) throw new ArgumentException($"Given coordinates (x:{coordinates.X}, y:{coordinates.Y}) are not in bounds (x:0-{Width}, y:0-{Height}).");
    }
}

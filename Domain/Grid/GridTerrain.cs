using System;
using System.Collections;
using ImaginedWorlds.Domain.Common;
using ImaginedWorlds.Domain.Common.Primitives;

namespace ImaginedWorlds.Domain.Grid;

public sealed class GridTerrain : AggregateRoot<Ulid>
{
    private byte[,] _grid;
    public IReadOnlyList<IReadOnlyList<byte>> GridView => new Grid2DView(_grid);
    public Grid2DView NativeGridView => new Grid2DView(_grid);
    public sealed class Grid2DView : IReadOnlyList<IReadOnlyList<byte>>
    {
        private readonly byte[,] _source;

        public Grid2DView(byte[,] source)
        {
            _source = source;
        }

        public IReadOnlyList<byte> this[int rowIndex] => new GridRowView(_source, rowIndex);
        public int Count => _source.GetLength(0);
        public int Height => Count;
        public int Width => this[0].Count;

        public IEnumerator<IReadOnlyList<byte>> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public byte[,] GetBytes()
    {
        return _grid;
    }
    private sealed class GridRowView : IReadOnlyList<byte>
    {
        private readonly byte[,] _source;
        private readonly int _rowIndex;

        public GridRowView(byte[,] source, int rowIndex)
        {
            _source = source;
            _rowIndex = rowIndex;
        }

        public byte this[int columnIndex] => _source[_rowIndex, columnIndex];
        public int Count => _source.GetLength(1);

        public IEnumerator<byte> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public int Width { get; private set; }
    public int Height { get; private set; }

    private GridTerrain(int width, int height)
    {
        if (width < 10 || height < 10) throw new ArgumentException("X and Y heights cannot be less than 10.");
        //if (xHeight != yHeight) throw new ArgumentException("X and Y heights has to be identical to keep square form");

        _grid = new byte[height, width];
    }

    public static GridTerrain Create(int width, int height)
    {
        return new(width, height);
    }

    public void SetTile(TilePatch tilePatch)
    {
        ArgumentNullException.ThrowIfNull(tilePatch);

        GridHelper.CheckBounds(Height, Width, tilePatch.Coordinates);

        if (_grid[tilePatch.Coordinates.Y, tilePatch.Coordinates.X] == (byte)tilePatch.TileType) return;

        _grid[tilePatch.Coordinates.Y, tilePatch.Coordinates.X] = (byte)tilePatch.TileType;

        AddDomainEvent(
            new GridTileSetDomainEvent(Id, tilePatch)
        );
    }

    public TileType GetTile(int x, int y)
    {
        GridHelper.CheckBounds(Height, Width, new(x, y));
        return (TileType)_grid[y, x];
    }
}

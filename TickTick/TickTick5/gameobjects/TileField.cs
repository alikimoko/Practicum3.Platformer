class TileField : GameObjectGrid
{
    /// <summary>Create the tile field.</summary>
    /// <param name="rows">The height of the field.</param>
    /// <param name="columns">The width of the field.</param>
    public TileField(int rows, int columns, int layer = 0, string id = "") : base(rows, columns, layer, id) { }

    /// <summary>Get the type of the tile at the given coördinates.</summary>
    public TileType GetTileType(int x, int y)
    {
        if (x < 0 || x >= Columns)
            return TileType.Normal;
        if (y < 0 || y >= Rows)
            return TileType.Background;
        Tile current = Objects[x, y] as Tile;
        return current.TileType;
    }
}
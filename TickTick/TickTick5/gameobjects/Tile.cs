using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

enum TileType
{
    Background,
    Normal,
    Platform
}

class Tile : SpriteGameObject
{
    protected TileType type;
    protected bool hot;
    protected bool ice;

    /// <summary>Create a new tile.</summary>
    /// <param name="assetname">The tile its sprite.</param>
    /// <param name="tp">The type thistile is.</param>
    public Tile(string assetname = "", TileType tp = TileType.Background, int layer = 0, string id = "") : base(assetname, layer, id, 0, true)
    {
        type = tp;
        hot = false;
        ice = false;
    }

    /// <summary>Draw the tile.</summary>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (type == TileType.Background)
            return;
        base.Draw(gameTime, spriteBatch);
    }

    /// <summary>Get the tile type.</summary>
    public TileType TileType
    { get { return type; } }

    /// <summary>Is this tile hot?</summary>
    public bool Hot
    {
        get { return hot; }
        set { hot = value; }
    }

    /// <summary>Is this tile made of ice?</summary>
    public bool Ice
    {
        get { return ice; }
        set { ice = value; }
    }
}

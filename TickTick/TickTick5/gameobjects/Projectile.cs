using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Projectile : AnimatedGameObject
{
    protected bool hit;
    protected bool active;

    public Projectile(Vector2 playerposition, bool mirrored) : base(0 , "", true)
    {
        velocity.X = mirrored ? -750 : 750;
        position = playerposition - new Vector2(15);
        hit = false;
        LoadAnimation("Sprites/ball-strip@15", "default", true, 15);
        PlayAnimation("default");
    }

    public override void Update(GameTime gameTime)
    {
        if (!active)
            return;
        
        base.Update(gameTime);

        TileField tiles = GameWorld.Find("tiles") as TileField;
        int x_floor = (int)position.X / tiles.CellWidth;
        int y_floor = (int)position.Y / tiles.CellHeight;
        for (int y = y_floor - 2; y <= y_floor + 1; ++y)
            for (int x = x_floor - 1; x <= x_floor + 1; ++x)
            {
                TileType tileType = tiles.GetTileType(x, y);
                if (tileType == TileType.Normal || tileType == TileType.Platform)
                {
                    Tile currentTile = tiles.Get(x, y) as Tile;
                    Rectangle tileBounds = new Rectangle(x * tiles.CellWidth, y * tiles.CellHeight,
                                                            tiles.CellWidth, tiles.CellHeight);
                    if (tileBounds.Intersects(BoundingBox))
                    { hit = true; }
                }
            }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (!visible || sprite == null)
            return;
        sprite.Draw(spriteBatch, position - ((GameEnvironment.ActiveCamera != null && moving) ? GameEnvironment.ActiveCamera.Position : Vector2.Zero), origin);
    }

    public bool Active
    {
        get { return active; }
        set { active = value; }
    }

    /// <summary>Indicates wether the projectile has hit something.</summary>
    public bool Hit
    {
        get { return hit; }
        set { hit = value; }
    }

    /// <summary>Get the bounding box for this object.</summary>
    public override Rectangle BoundingBox
    {
        get
        {
            int left = (int)(position.X - origin.X);
            int top = (int)(position.Y - origin.Y);
            return new Rectangle(left, top, Width, Height);
        }
    }
}


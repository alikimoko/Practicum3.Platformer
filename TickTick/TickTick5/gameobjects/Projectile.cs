using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Projectile : AnimatedGameObject
{
    protected bool hit;
    protected bool active;
    protected bool mirrored;

    public Projectile(Vector2 playerposition, bool mirrored) : base(0 , "", true)
    {
        this.mirrored = mirrored;
        velocity.X = mirrored ? -750 : 750;
        position = playerposition - new Vector2(15);
        hit = false;
        active = true;
        LoadAnimation("Sprites/ball-strip@15", "default", true, 15);
        PlayAnimation("default");
    }

    public void Update(GameTime gameTime, TileField tiles)
    {
        if (!active)
            return;
        base.Update(gameTime);

        int x_floor = (int)position.X / tiles.CellWidth;
        int y_floor = (int)position.Y / tiles.CellHeight;
        for (int y = y_floor - 1; y <= y_floor + 1; y++)
            for (int x = x_floor - (mirrored ? 1 : 0); x <= x_floor + (mirrored ? 0 : 1); x++)
            {
                TileType tileType = tiles.GetTileType(x, y);
                if (tileType == TileType.Normal || tileType == TileType.Platform)
                {
                    Rectangle tileBounds = new Rectangle(x * tiles.CellWidth, y * tiles.CellHeight,
                                                            tiles.CellWidth, tiles.CellHeight);
                    if (tileBounds.Intersects(BoundingBox))
                    { hit = true; }
                }
            }
        if (hit)
            active = false;
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);
        if (hit)
            visible = false;
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
}


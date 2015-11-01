using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Projectile : AnimatedGameObject
{
    protected bool hit;
    protected bool active;
    protected bool mirrored;
    
    /// <summary>Create a new projectile.</summary>
    /// <param name="playerposition">The position of the player.</param>
    /// <param name="mirrored">Should the projectile go left?</param>
  public Projectile(Vector2 playerposition, bool mirrored) : base(0 , "", Backgroundlayer.foreground)
   
   
    {
        this.mirrored = mirrored;
        velocity.X = mirrored ? -750 : 750;
        position = playerposition - new Vector2(15);
        hit = false;
        active = true;
        LoadAnimation("Sprites/ball-strip@15", "default", true, 15);
        PlayAnimation("default");
        Mirror = mirrored;
    }

    /// <summary>Update the projectile.</summary>
    /// <param name="tiles">The field of the level.</param>
    public void Update(GameTime gameTime, TileField tiles)
    {
        if (!active)
            return;
        base.Update(gameTime);

        // check bounds 
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

    /// <summary>Draw the projectile.</summary>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);
        if (hit) // make sure you see it hit.
            visible = false;
    }

    /// <summary>Is the projectile active?</summary>
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


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Rocket : AnimatedGameObject
{
    protected double spawnTime;
    protected Vector2 startPosition;
    protected const int maxhp = 3;
    protected int hp;
    
    /// <summary>Create a new rocket.</summary>
    /// <param name="moveToLeft">Is the rocket moving to the left?</param>
    /// <param name="startPosition">The starting position of the rocket.</param>
    public Rocket(bool moveToLeft, Vector2 startPosition) : base (0,"", Backgroundlayer.foreground)
    {
        LoadAnimation("Sprites/Rocket/spr_rocket@3", "default", true, 0.2f);
        PlayAnimation("default");
        Mirror = moveToLeft;
        this.startPosition = startPosition;
        Reset();
    }
    
    /// <summary>Reset the rocket.</summary>
    public override void Reset()
    {
        Visible = false;
        position = startPosition;
        velocity = Vector2.Zero;
        spawnTime = GameEnvironment.Random.NextDouble() * 4 + 1;
        hp = maxhp;
    }

    /// <summary>Update the rocket.</summary>
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (spawnTime > 0)
        {
            spawnTime -= gameTime.ElapsedGameTime.TotalSeconds;
            return;
        }
        Visible = true;
        velocity.X = Mirror ? -600 : 600;
        CheckCollisions();

        // check if we are outside the level
        Level level = parent.Parent as Level; // level > enemies > rocket
        Rectangle levelBox = new Rectangle(0, 0, level.Width, level.Height);
        if (!levelBox.Intersects(BoundingBox))
            Reset();
    }

    /// <summary>Draw the rocket.</summary>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);

        // health bar
        if (visible && hp != maxhp)
            DrawingHelper.DrawStatusBar(spriteBatch, GlobalPosition - new Vector2(0, 100) - GameEnvironment.ActiveCamera.Position,
                                        new Vector2(100, 10), maxhp, hp,
                                        new Color(255 - (int)(255 * (float)hp / maxhp), (int)(255 * (float)hp / maxhp), 0));
    }

    /// <summary>Check and handle the collisions.</summary>
    public void CheckCollisions()
    {
        TileField tiles = GameWorld.Find("tiles") as TileField;
        int x_floor = (int)position.X / tiles.CellWidth;
        int y_floor = (int)position.Y / tiles.CellHeight;

        // tile collisions
        for (int y = y_floor - 2; y <= y_floor + 1; ++y)
            for (int x = x_floor - 1; x <= x_floor + 1; ++x)
            {
                if (y < 0 || y >= tiles.Rows || x < 0 || x >= tiles.Columns)
                    continue;

                TileType tileType = tiles.GetTileType(x, y);
                if (tileType == TileType.Normal || tileType == TileType.Platform)
                {
                    Rectangle tileBounds = new Rectangle(x * tiles.CellWidth, y * tiles.CellHeight,
                                                            tiles.CellWidth, tiles.CellHeight);
                    if (tileBounds.Intersects(BoundingBox))
                    {
                        Reset();
                        return;
                    }
                }
            }

        Player player = GameWorld.Find("player") as Player;

        // projectile collisions
        foreach (Projectile projectile in player.Projectiles)
            if (BoundingBox.Intersects(projectile.BoundingBox) && projectile.Active)
            {
                projectile.Hit = true;
                hp--;
                if (hp <= 0)
                {
                    Reset();
                    return;
                }
            }

        // player collision
        if (CollidesWith(player))
        {
            Vector2 depth = Collision.CalculateIntersectionDepth(BoundingBox, player.BoundingBox);
            if (player.BoundingBox.Y < BoundingBox.Y && player.Velocity.Y > 0 &&  depth.X > depth.Y )
            { velocity.Y = 600; player.Jump(); }
            else
            {
                player.LowerHP(50);
                Reset();
            }
        }
    }
}

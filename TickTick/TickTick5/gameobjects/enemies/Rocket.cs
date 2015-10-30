using Microsoft.Xna.Framework;

class Rocket : AnimatedGameObject
{
    protected double spawnTime;
    protected Vector2 startPosition;
    
    public Rocket(bool moveToLeft, Vector2 startPosition) : base (0,"", true)
    {
        LoadAnimation("Sprites/Rocket/spr_rocket@3", "default", true, 0.2f);
        PlayAnimation("default");
        Mirror = moveToLeft;
        this.startPosition = startPosition;
        Reset();
    }
    
    public override void Reset()
    {
        Visible = false;
        position = startPosition;
        velocity = Vector2.Zero;
        spawnTime = GameEnvironment.Random.NextDouble() * 4 + 1;
        
    }

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

        // check if we are outside the screen
        Level level = parent.Parent as Level; // level > enemies > rocket
        Rectangle levelBox = new Rectangle(0, 0, level.Width, level.Height);
        if (!levelBox.Intersects(BoundingBox))
            Reset();
    }

    public void CheckCollisions()
    {
        Player player = GameWorld.Find("player") as Player;

        foreach (Projectile projectile in player.Projectiles)
            if (BoundingBox.Intersects(projectile.BoundingBox) && projectile.Active)
            {
                Reset();
                projectile.Hit = true;
            }

        if (CollidesWith(player))
            player.Die(false);
    }
}

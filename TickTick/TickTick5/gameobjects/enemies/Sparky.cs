using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Sparky : AnimatedGameObject
{
    protected float idleTime;
    protected float yoffset;
    protected float initialY;
    protected bool active;
    protected const int maxhp = 5;
    protected int hp;

    /// <summary>Create a new sparky.</summary>
    /// <param name="initialY">The y to place the sparky.</param>
    public Sparky(float initialY) : base(0,"", Backgroundlayer.foreground)
    {
        LoadAnimation("Sprites/Sparky/spr_electrocute@6x5", "electrocute", false, 0.1f);
        LoadAnimation("Sprites/Sparky/spr_idle", "idle", true, 0.1f);
        PlayAnimation("idle");
        this.initialY = initialY;
        active = true;
        Reset();
    }

    /// <summary>Reset sparky.</summary>
    public override void Reset()
    {
        idleTime = (float)GameEnvironment.Random.NextDouble() * 5;
        position.Y = initialY;
        yoffset = 120;
        velocity = Vector2.Zero;
        active = true;
        visible = true;
        hp = maxhp;
    }

    /// <summary>Update sparky.</summary>
    public override void Update(GameTime gameTime)
    {
        if (!active)
            return;

        base.Update(gameTime);
        if (idleTime <= 0)
        {
            PlayAnimation("electrocute");
            if (velocity.Y != 0)
            {
                // falling down
                yoffset -= velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (yoffset <= 0)
                    velocity.Y = 0;
                else if (yoffset >= 120.0f)
                    Reset();
            }
            else if (Current.AnimationEnded)
                velocity.Y = -60;
        }
        else
        {
            PlayAnimation("idle");
            idleTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (idleTime <= 0.0f)
                velocity.Y = 300;
        }

        CheckCollisions();
    }

    /// <summary>Draw sparky.</summary>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);

        // health bar
        if (visible && hp != maxhp)
            DrawingHelper.DrawStatusBar(spriteBatch, GlobalPosition - new Vector2(0, 200) - GameEnvironment.ActiveCamera.Position,
                                        new Vector2(100, 10), maxhp, hp,
                                        new Color(255 - (int)(255 * (float)hp / maxhp), (int)(255 * (float)hp / maxhp), 0));
    }

    /// <summary>Check and handle the collisions.</summary>
    public void CheckCollisions()
    {
        Player player = GameWorld.Find("player") as Player;

        // projectile collisions
        foreach (Projectile projectile in player.Projectiles)
            if (BoundingBox.Intersects(projectile.BoundingBox) && projectile.Active)
            {
                projectile.Hit = true;
                hp--;
                if (hp <= 0)
                {
                    visible = false;
                    active = false;
                    return;
                }
            }

        // player collisions
        if (CollidesWith(player) && idleTime <= 0.0f)
            player.LowerHP(2);
    }
}
using Microsoft.Xna.Framework;

class Sparky : AnimatedGameObject
{
    protected float idleTime;
    protected float yoffset;
    protected float initialY;
    protected bool active;
    protected const int maxhp = 5;
    protected int hp;

    public Sparky(float initialY) : base(0,"", true)
    {
        LoadAnimation("Sprites/Sparky/spr_electrocute@6x5", "electrocute", false, 0.1f);
        LoadAnimation("Sprites/Sparky/spr_idle", "idle", true, 0.1f);
        PlayAnimation("idle");
        this.initialY = initialY;
        active = true;
        Reset();
    }

    public override void Reset()
    {
        idleTime = (float)GameEnvironment.Random.NextDouble() * 5;
        position.Y = initialY;
        yoffset = 120;
        velocity = Vector2.Zero;
        if (hp <= 0)
        {
            active = true;
            visible = true;
            hp = maxhp;
        }
    }

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

    public void CheckCollisions()
    {
        Player player = GameWorld.Find("player") as Player;

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

        if (CollidesWith(player) && idleTime <= 0.0f)
            player.Die(false);
    }
}
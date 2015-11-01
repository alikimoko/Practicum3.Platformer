using Microsoft.Xna.Framework;

class Turtle : AnimatedGameObject
{
    protected float sneezeTime;
    protected float idleTime;

    /// <summary>Create a net turtle.</summary>
    public Turtle() : base(0, "", Backgroundlayer.foreground)
    {
        LoadAnimation("Sprites/Turtle/spr_sneeze@9", "sneeze", false, 0.1f);
        LoadAnimation("Sprites/Turtle/spr_idle", "idle", true, 0.1f);
        PlayAnimation("idle");
        Reset();
    }

    /// <summary>Reset the turtle.</summary>
    public override void Reset()
    {
        sneezeTime = 0.0f;
        idleTime = 5.0f;
    }

    /// <summary>Update the turtle.</summary>
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (sneezeTime > 0)
        {
            PlayAnimation("sneeze");
            sneezeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (sneezeTime <= 0.0f)
            {
                idleTime = 5.0f;
                sneezeTime = 0.0f;
            }
        }
        else if (idleTime > 0)
        {
            PlayAnimation("idle");
            idleTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (idleTime <= 0.0f)
            {
                idleTime = 0.0f;
                sneezeTime = 5.0f;
            }
        }

        CheckCollisions();
    }

    /// <summary>Check and handle the collisions.</summary>
    public void CheckCollisions()
    {
        Player player = GameWorld.Find("player") as Player;

        foreach (Projectile projectile in player.Projectiles)
            if (BoundingBox.Intersects(projectile.BoundingBox) && projectile.Active)
                projectile.Hit = true;

        if (!CollidesWith(player))
            return;
        if (sneezeTime > 0)
            player.Die(false);
        else if (idleTime > 0 && player.Velocity.Y > 0)
            player.Jump(1500);
    }
}
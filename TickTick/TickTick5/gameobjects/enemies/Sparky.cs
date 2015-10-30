using System.Collections.Generic;
using Microsoft.Xna.Framework;

class Sparky : AnimatedGameObject
{
    protected float idleTime;
    protected float yoffset;
    protected float initialY;

    public Sparky(float initialY) : base(0,"", true)
    {
        LoadAnimation("Sprites/Sparky/spr_electrocute@6x5", "electrocute", false, 0.1f);
        LoadAnimation("Sprites/Sparky/spr_idle", "idle", true, 0.1f);
        PlayAnimation("idle");
        this.initialY = initialY;
        Reset();
    }

    public override void Reset()
    {
        idleTime = (float)GameEnvironment.Random.NextDouble() * 5;
        position.Y = initialY;
        yoffset = 120;
        velocity = Vector2.Zero;
    }

    public override void Update(GameTime gameTime)
    {
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

        CheckPlayerCollision();
    }

    public void CheckPlayerCollision()
    {
        Player player = GameWorld.Find("player") as Player;
        if (CollidesWith(player) && idleTime <= 0.0f)
            player.Die(false);
    }
}
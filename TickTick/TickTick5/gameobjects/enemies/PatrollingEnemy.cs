using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class PatrollingEnemy : AnimatedGameObject
{
    protected float waitTime;
    protected bool active;
    protected const int maxhp = 5;
    protected int hp;

    /// <summary>Create a new patrolling enemy (flame).</summary>
    public PatrollingEnemy() : base(0,"", Backgroundlayer.foreground)
    {
        waitTime = 0.0f;
        velocity.X = 120;
        LoadAnimation("Sprites/Flame/spr_flame@9", "default", true, 0.1f);
        PlayAnimation("default");
        Reset();
    }

    /// <summary>Update the flame.</summary>
    public override void Update(GameTime gameTime)
    {
        if (!active)
            return;

        base.Update(gameTime);
        if (waitTime > 0)
        {
            waitTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (waitTime <= 0.0f)
                TurnAround();
        }
        else
        {
            // move bounds
            TileField tiles = GameWorld.Find("tiles") as TileField;
            float posX = BoundingBox.Left;
            if (!Mirror)
                posX = BoundingBox.Right;
            int tileX = (int)Math.Floor(posX / tiles.CellWidth);
            int tileY = (int)Math.Floor(position.Y / tiles.CellHeight);
            if (tiles.GetTileType(tileX, tileY - 1) == TileType.Normal ||
                tiles.GetTileType(tileX, tileY) == TileType.Background)
            {
                waitTime = 0.5f;
                velocity.X = 0.0f;
            }
        }
        CheckCollisions();
    }

    /// <summary>Draw the flame.</summary>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);

        if (visible && hp != maxhp)
            DrawingHelper.DrawStatusBar(spriteBatch, GlobalPosition - new Vector2(0, 150) - GameEnvironment.ActiveCamera.Position,
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

        // player collision
        if (CollidesWith(player))
            player.LowerHP(5);
    }

    /// <summary>Turnand startmoving in the oposite direction.</summary>
    public void TurnAround()
    {
        Mirror = !Mirror;
        velocity.X = 120;
        if (Mirror)
            velocity.X = -velocity.X;
    }

    /// <summary>Reset the flame</summary>
    public override void Reset()
    {
        base.Reset();
        active = true;
        hp = maxhp;
    }
}

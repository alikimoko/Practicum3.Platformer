using Microsoft.Xna.Framework;
using System;

class PatrollingEnemy : AnimatedGameObject
{
    protected float waitTime;

    public PatrollingEnemy() : base(0,"", true)
    {
        waitTime = 0.0f;
        velocity.X = 120;
        LoadAnimation("Sprites/Flame/spr_flame@9", "default", true, 0.1f);
        PlayAnimation("default");
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (waitTime > 0)
        {
            waitTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (waitTime <= 0.0f)
                TurnAround();
        }
        else
        {
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
        CheckPlayerCollision();
    }

    public void CheckPlayerCollision()
    {
        Player player = GameWorld.Find("player") as Player;
        if (CollidesWith(player))
            player.Die(false);
    }

    public void TurnAround()
    {
        Mirror = !Mirror;
        velocity.X = 120;
        if (Mirror)
            velocity.X = -velocity.X;
    }
}

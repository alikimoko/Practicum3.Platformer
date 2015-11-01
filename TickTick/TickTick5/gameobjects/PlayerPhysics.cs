using System;
using Microsoft.Xna.Framework;

partial class Player :  AnimatedGameObject
{
    /// <summary>Make the player jump.</summary>
    /// <param name="speed">The vertical speed the jump causes.</param>
    public void Jump(float speed = 1100)
    {
        velocity.Y = -speed;
        GameEnvironment.AssetManager.PlaySound("Sounds/snd_player_jump");
    }
    
    /// <summary>Handle the player physics.</summary>
    private void DoPhysics()
    {
        if (!exploded)
            velocity.Y += 55;
        if (isAlive)
            HandleCollisions();
    }

    /// <summary>Handle the player collisions.</summary>
    private void HandleCollisions()
    {
        isOnTheGround = false;
        walkingOnIce = false;
        walkingOnHot = false;

        TileField tiles = GameWorld.Find("tiles") as TileField;
        int x_floor = (int)position.X / tiles.CellWidth;
        int y_floor = (int)position.Y / tiles.CellHeight;

        for (int y = y_floor - 2; y <= y_floor + 1; ++y)
            for (int x = x_floor - 1; x <= x_floor + 1; ++x)
            {
                TileType tileType = tiles.GetTileType(x, y);
                if (tileType == TileType.Background)
                    continue;
                // solid tile
                Tile currentTile = tiles.Get(x, y) as Tile;
                Rectangle tileBounds = new Rectangle(x * tiles.CellWidth, y * tiles.CellHeight,
                                                        tiles.CellWidth, tiles.CellHeight);
                Rectangle boundingBox = BoundingBox;
                boundingBox.Height += 1;
                if (((currentTile != null && !currentTile.CollidesWith(this)) || currentTile == null) && !tileBounds.Intersects(boundingBox))
                    continue;
                // colliding
                Vector2 depth = Collision.CalculateIntersectionDepth(boundingBox, tileBounds);
                if (Math.Abs(depth.X) < Math.Abs(depth.Y))
                {
                    if (tileType == TileType.Normal)
                        position.X += depth.X;
                    continue;
                }
                if (previousYPosition <= tileBounds.Top && tileType != TileType.Background)
                {
                    if (velocity.Y > 1250) // fall damage
                        LowerHP(((int)velocity.Y - 1250) / 100);
                    isOnTheGround = true;
                    velocity.Y = 0;
                    if (currentTile != null) // update status effects
                    {
                        walkingOnIce = currentTile.Ice;
                        walkingOnHot = currentTile.Hot;
                    }
                }
                if (tileType == TileType.Normal || isOnTheGround)
                    position.Y += depth.Y + 1; // make sure we stand on top of the tile
            }
        position = new Vector2((int)position.X, (int)position.Y);
        previousYPosition = position.Y;
    }
}
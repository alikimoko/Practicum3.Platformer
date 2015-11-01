using System;
using Microsoft.Xna.Framework;

class WaterDrop : SpriteGameObject
{
    protected float bounce;

    /// <summary>Create the water drop.</summary>
    public WaterDrop(int layer=0, string id="") : base("Sprites/spr_water", layer, id, 0, true) { }

    /// <summary>Update the water drop.</summary>
    public override void Update(GameTime gameTime)
    {
        double t = gameTime.TotalGameTime.TotalSeconds * 3.0f + Position.X;
        bounce = (float)Math.Sin(t) * 0.2f;
        position.Y += bounce;
        Player player = GameWorld.Find("player") as Player;
        if (visible && CollidesWith(player))
        {
            visible = false;
            GameEnvironment.AssetManager.PlaySound("Sounds/snd_watercollected");
        }
    }
}

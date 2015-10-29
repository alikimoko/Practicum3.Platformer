using Microsoft.Xna.Framework;

class Projectile : SpriteGameObject
{
    public Projectile(Vector2 playerposition, bool mirrored) : base("projectile", 0 , "", 0, true)
    {
        velocity.X = mirrored ? -50 : 50;
        position = playerposition;
    }
}


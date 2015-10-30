using Microsoft.Xna.Framework;

class Projectile : AnimatedGameObject
{
    public Projectile(Vector2 playerposition, bool mirrored) : base(0 , "", true)
    {
        velocity.X = mirrored ? -500 : 500;
        position = playerposition - new Vector2(15);
        LoadAnimation("Sprites/ball-strip@15", "default", true, 15);
        PlayAnimation("default");
    }
}


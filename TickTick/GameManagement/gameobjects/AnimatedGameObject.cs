using System.Collections.Generic;
using Microsoft.Xna.Framework;

public class AnimatedGameObject : SpriteGameObject
{
    protected Dictionary<string,Animation> animations;

    /// <summary>Create a new animated game object.</summary>
    /// <param name="paralax">The paralax mode that applies to this object.</param>
    public AnimatedGameObject(int layer = 0, string id = "", Backgroundlayer paralax = Backgroundlayer.solid) : base("", layer, id, 0, paralax)
    { animations = new Dictionary<string, Animation>(); }

    /// <summary>Load a new animation.</summary>
    /// <param name="assetname">The name of the animation file.</param>
    /// <param name="id">The name of the animation.</param>
    /// <param name="looping">Should the animation be looped?</param>
    /// <param name="fps">The FPS of the animation.</param>
    public void LoadAnimation(string assetname, string id, bool looping, int fps = 10)
    {
        Animation anim = new Animation(assetname, looping, fps);
        animations[id] = anim;
    }

    /// <summary>Load a new animation.</summary>
    /// <param name="assetname">The name of the animation file.</param>
    /// <param name="id">The name of the animation.</param>
    /// <param name="looping">Should the animation be looped?</param>
    /// <param name="frametime">The time per frame in the animation.</param>
    public void LoadAnimation(string assetname, string id, bool looping, float frametime = 0.1f)
    {
        Animation anim = new Animation(assetname, looping, frametime);
        animations[id] = anim;        
    }

    /// <summary>Play the given animation.</summary>
    public void PlayAnimation(string id)
    {
        if (sprite == animations[id])
            return;
        if (sprite != null)
            animations[id].Mirror = sprite.Mirror;
        animations[id].Play();
        sprite = animations[id];
        origin = new Vector2(sprite.Width / 2, sprite.Height);        
    }

    /// <summary>Update the animation.</summary>
    public override void Update(GameTime gameTime)
    {
        if (sprite == null)
            return;
        Current.Update(gameTime);
        base.Update(gameTime);
    }

    /// <summary>Get teh current animation.</summary>
    public Animation Current
    { get { return sprite as Animation; } }
}
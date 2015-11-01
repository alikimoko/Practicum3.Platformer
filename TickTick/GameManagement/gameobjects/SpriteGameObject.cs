using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class SpriteGameObject : GameObject
{
    protected SpriteSheet sprite;
    protected Vector2 origin;
    protected bool moving;
    public enum Backgroundlayer { solid = 0 , foreground = 1, middle = 10 , background = 25 };
    protected Backgroundlayer paralax;

    /// <summary>Create a new sprite game object.</summary>
    /// <param name="assetname">The name of the sprite file.</param>
    /// <param name="layer">The layer the object is in.</param>
    /// <param name="id">The ID to refer to the object.</param>
    /// <param name="sheetIndex">The index of the sprite in a sprite string or sheet.</param>
    public SpriteGameObject(string assetname, int layer = 0, string id = "", int sheetIndex = 0, Backgroundlayer paralax = Backgroundlayer.solid)
        : base(layer, id)
    {
        if (assetname != "")
            sprite = new SpriteSheet(assetname, sheetIndex);
        else
            sprite = null;
        this.paralax = paralax;
    }    

    /// <summary>Draw this object.</summary>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (!visible || sprite == null)
            return;
        sprite.Draw(spriteBatch, GlobalPosition - ((GameEnvironment.ActiveCamera != null&& (int)paralax != 0) ? GameEnvironment.ActiveCamera.Position / (int)paralax : Vector2.Zero), origin);
    }

    /// <summary>Get the sprite for this object.</summary>
    public SpriteSheet Sprite
    { get { return sprite; } }

    /// <summary>Get the centre of this object.</summary>
    public Vector2 Center
    { get { return new Vector2(Width, Height) / 2; } }

    /// <summary>Get the width of this object.</summary>
    public int Width
    { get { return sprite.Width; } }

    /// <summary>Get the height of this object.</summary>
    public int Height
    { get { return sprite.Height; } }

    /// <summary>Is the sprite for this object mirrored?</summary>
    public bool Mirror
    {
        get { return sprite.Mirror; }
        set { sprite.Mirror = value; }
    }

    /// <summary>The origin for the sprite for this object.</summary>
    public Vector2 Origin
    {
        get { return origin; }
        set { origin = value; }
    }

    /// <summary>Get the bounding box for this object.</summary>
    public override Rectangle BoundingBox
    {
        get
        {
            int left = (int)(GlobalPosition.X - origin.X);
            int top = (int)(GlobalPosition.Y - origin.Y);
            return new Rectangle(left, top, Width, Height);
        }
    }

    /// <summary>Is this object colliding with a given object?</summary>
    /// <param name="obj">The object to check collision with.</param>
    public bool CollidesWith(SpriteGameObject obj)
    {
        if (!Visible || !obj.Visible || !Collision.IsColliding(BoundingBox, obj.BoundingBox))
            return false;
        Rectangle b = Collision.Intersection(BoundingBox, obj.BoundingBox);
        for (int x = 0; x < b.Width; x++)
            for (int y = 0; y < b.Height; y++)
            {
                int thisx = b.X - (int)(GlobalPosition.X - origin.X) + x;
                int thisy = b.Y - (int)(GlobalPosition.Y - origin.Y) + y;
                int objx = b.X - (int)(obj.GlobalPosition.X - obj.origin.X) + x;
                int objy = b.Y - (int)(obj.GlobalPosition.Y - obj.origin.Y) + y;
                if (sprite.GetPixelColor(thisx, thisy).A != 0 &&
                    obj.sprite.GetPixelColor(objx, objy).A != 0)
                    return true;
            }
        return false;
    }
}


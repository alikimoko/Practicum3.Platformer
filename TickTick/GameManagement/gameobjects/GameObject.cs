using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class GameObject : IGameLoopObject
{
    protected GameObject parent;
    protected Vector2 position, velocity;
    protected int layer;
    protected string id;
    protected bool visible;

    /// <summary>Create a new game object.</summary>
    /// <param name="layer">The layer the object is in.</param>
    /// <param name="id">The ID to refer to the object.</param>
    public GameObject(int layer = 0, string id = "")
    {
        this.layer = layer;
        this.id = id;
        position = Vector2.Zero;
        velocity = Vector2.Zero; 
        visible = true;
    }

    public virtual void HandleInput(InputHelper inputHelper) { }

    /// <summary>Update the object its position.</summary>
    public virtual void Update(GameTime gameTime)
    { position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds; }

    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }

    /// <summary>Reset the object its visibility.</summary>
    public virtual void Reset()
    { visible = true; }

    /// <summary>The position of the object in its reference frame.</summary>
    public virtual Vector2 Position
    {
        get { return position; }
        set { position = value; }
    }

    /// <summary>The velocity of the object.</summary>
    public virtual Vector2 Velocity
    {
        get { return velocity; }
        set { velocity = value; }
    }

    /// <summary>Get the absolute position of the object.</summary>
    public virtual Vector2 GlobalPosition
    {
        get
        {
            if (parent != null)
                return parent.GlobalPosition + position;
            else
                return position;
        }
    }

    /// <summary>Get this objects root object.</summary>
    public GameObject Root
    {
        get
        {
            if (parent != null)
                return parent.Root;
            else
                return this;
        }
    }

    /// <summary>Get the game world this object is in.</summary>
    public GameObjectList GameWorld
    { get { return Root as GameObjectList; } }

    /// <summary>the layer this object is in.</summary>
    public virtual int Layer
    {
        get { return layer; }
        set { layer = value; }
    }

    /// <summary>The parent of this object.</summary>
    public virtual GameObject Parent
    {
        get { return parent; }
        set { parent = value; }
    }

    /// <summary>Get the reference ID of this object.</summary>
    public string ID
    { get { return id; } }

    /// <summary>Is this object visible?</summary>
    public bool Visible
    {
        get { return visible; }
        set { visible = value; }
    }

    /// <summary>Get this object its bounding box.</summary>
    public virtual Rectangle BoundingBox
    { get { return new Rectangle((int)GlobalPosition.X, (int)GlobalPosition.Y, 0, 0); } }
}
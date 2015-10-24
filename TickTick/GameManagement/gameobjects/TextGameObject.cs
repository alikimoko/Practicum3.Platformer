using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class TextGameObject : GameObject
{
    protected SpriteFont spriteFont;
    protected Color color;
    protected string text;
    protected bool centered;

    /// <summary>Create a new text object.</summary>
    /// <param name="assetname">The name of the sprite font to use.</param>
    /// <param name="layer">The layer to draw the text in.</param>
    /// <param name="id">The reference ID for this text object.</param>
    public TextGameObject(string assetname, int layer = 0, string id = "")
        : base(layer, id)
    {
        spriteFont = GameEnvironment.AssetManager.Content.Load<SpriteFont>(assetname);
        color = Color.White;
        text = "";
        centered = false;
    }

    /// <summary>Draw the text.</summary>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (visible)
            if (centered)
                DrawingHelper.drawCenteredString(spriteBatch, spriteFont, color, text, GlobalPosition);
            else
                spriteBatch.DrawString(spriteFont, text, GlobalPosition, color);
    }

    /// <summary>The color to draw the text in.</summary>
    public Color Color
    {
        get { return color; }
        set { color = value; }
    }

    /// <summary>The text to draw.</summary>
    public string Text
    {
        get { return text; }
        set { text = value; }
    }

    /// <summary>Should the text be center aligned?</summary>
    public bool Centered
    {
        get { return centered; }
        set { centered = value; }
    }

    /// <summary>Get the size of the text object when drawn.</summary>
    public Vector2 Size
    { get { return spriteFont.MeasureString(text); } }
}


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class SpriteSheet
{
    protected Texture2D sprite;
    protected int sheetIndex;
    protected int sheetColumns;
    protected int sheetRows;
    protected bool mirror;

    /// <summary>Add a new sprite sheet.</summary>
    /// <param name="assetname"></param>
    /// <param name="sheetIndex"></param>
    public SpriteSheet(string assetname, int sheetIndex = 0)
    {
        sprite = GameEnvironment.AssetManager.GetSprite(assetname);
        this.sheetIndex = sheetIndex;
        sheetColumns = 1;
        sheetRows = 1;

        // see if we can extract the number of sheet elements from the assetname
        string[] assetSplit = assetname.Split('@');
        if (assetSplit.Length <= 1)
            return;

        string sheetNrData = assetSplit[assetSplit.Length - 1];
        string[] colrow = sheetNrData.Split('x');
        sheetColumns = int.Parse(colrow[0]);
        if (colrow.Length == 2)
            sheetRows = int.Parse(colrow[1]);
    }

    /// <summary>Draw the sprite.</summary>
    /// <param name="position">Where to draw the sprite.</param>
    /// <param name="origin">The offset in drawing the sprite.</param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 origin)
    {
        int columnIndex = sheetIndex % sheetColumns;
        int rowIndex = sheetIndex / sheetColumns % sheetRows;
        Rectangle spritePart = new Rectangle(columnIndex * Width, rowIndex * Height, Width, Height);
        spriteBatch.Draw(sprite, position, spritePart, Color.White,
            0.0f, origin, 1.0f, mirror ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.0f);
    }

    /// <summary>Get the color of a pixel in the sprite at coördinates x and y.</summary>
    public Color GetPixelColor(int x, int y)
    {
        int column_index = sheetIndex % sheetColumns;
        int row_index = sheetIndex / sheetColumns % sheetRows;
        Rectangle sourceRectangle = new Rectangle(column_index * Width + x, row_index * Height + y, 1, 1);
        Color[] retrievedColor = new Color[1];
        sprite.GetData(0, sourceRectangle, retrievedColor, 0, 1);
        return retrievedColor[0];
    }

    /// <summary>Get the sprite (sheet).</summary>
    public Texture2D Sprite
    { get { return sprite; } }

    /// <summary>Get the centre of the sprite.</summary>
    public Vector2 Center
    { get { return new Vector2(Width, Height) / 2; } }

    /// <summary>Get the width of the sprite.</summary>
    public int Width
    { get { return sprite.Width / sheetColumns; } }

    /// <summary>Get the heigth of the sprite.</summary>
    public int Height
    { get { return sprite.Height / sheetRows; } }

    /// <summary>Is the sprite mirrored?</summary>
    public bool Mirror
    {
        get { return mirror; }
        set { mirror = value; }
    }

    /// <summary>The index of the sprite in the sprite sheet.</summary>
    public int SheetIndex
    {
        get
        { return sheetIndex; }
        set
        {
            if (value < sheetColumns * sheetRows && value >= 0)
                sheetIndex = value;
        }
    }

    /// <summary>Get the number of sprites in the sprite sheet</summary>
    public int NumberSheetElements
    { get { return this.sheetColumns * this.sheetRows; } }
}
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class DrawingHelper
{
    protected static Texture2D pixel;

    /// <summary>Creates a 1x1 white pixel for pixel based drawing.</summary>
    public static void Initialize(GraphicsDevice graphics)
    {
        pixel = new Texture2D(graphics, 1, 1);
        pixel.SetData(new[] { Color.White });
    }

    /// <summary>Draws the given rectangle.</summary>
    /// <param name="r">The rectangle to draw.</param>
    /// <param name="col">The color to draw the rectangle in.</param>
    /// <param name="bw">The border width to draw the rectangle with. (shadow to botom-right)</param>
    public static void DrawRectangle(Rectangle r, SpriteBatch spriteBatch, Color col, int bw = 2)
    {
        spriteBatch.Draw(pixel, new Rectangle(r.Left, r.Top, bw, r.Height), col); // Left
        spriteBatch.Draw(pixel, new Rectangle(r.Right, r.Top, bw, r.Height), col); // Right
        spriteBatch.Draw(pixel, new Rectangle(r.Left, r.Top, r.Width, bw), col); // Top
        spriteBatch.Draw(pixel, new Rectangle(r.Left, r.Bottom, r.Width, bw), col); // Bottom
    }

    /// <summary>Draw a centered text line.</summary>
    /// <param name="font">The font to use.</param>
    /// <param name="fontColor">The color to draw the text in.</param>
    /// <param name="text">The text to draw.</param>
    /// <param name="YStart">The y cordinate to start drawing the first line.</param>
    /// <param name="centre">The x cordinate to centre the text arround.</param>
    /// <param name="lineSpacing">The hight of a line. -1 uses default font height</param>
    /// <returns>The total height of the text.</returns>
    public static int drawCenteredString(SpriteBatch spriteBatch, SpriteFont font, Color fontColor, string text, float YStart, float Xcentre, int lineSpacing = -1)
    {
        if (!text.Contains("\n"))
        {
            // 1 line of text
            // temporary spacing
            int originalLineSpacing = font.LineSpacing;
            if (lineSpacing != -1) { font.LineSpacing = lineSpacing; }

            // string drawing
            Vector2 stringSize = font.MeasureString(text);
            spriteBatch.DrawString(font, text, new Vector2(Xcentre, YStart), fontColor, 0, new Vector2(stringSize.X / 2, 0), 1, SpriteEffects.None, 0);

            // return to normal spacing
            if (lineSpacing != -1) { font.LineSpacing = originalLineSpacing; }
            return (int)stringSize.Y;
        }
        else
        {
            // multiple lines of text
            return drawCenteredString(spriteBatch, font, fontColor, text.Split(new string[] { "\n" }, StringSplitOptions.None), YStart, Xcentre, lineSpacing);
        }
    }

    /// <summary>Draw a centered text line.</summary>
    /// <param name="font">The font to use.</param>
    /// <param name="fontColor">The color to draw the text in.</param>
    /// <param name="text">The text to draw.</param>
    /// <param name="textBox">A rectangle specifying the place the text should be drawn. (The text may go outside this box if it doesn't fit.)</param>
    /// <param name="lineSpacing">The hight of a line. -1 uses default font height</param>
    /// <returns>The total height of the text.</returns>
    public static int drawCenteredString(SpriteBatch spriteBatch, SpriteFont font, Color fontColor, string text, Rectangle textBox, int lineSpacing = -1)
    {
        return drawCenteredString(spriteBatch, font, fontColor, text, textBox.Top, textBox.Left + textBox.Width / 2, lineSpacing);
    }

    /// <summary>Draw a centered text line.</summary>
    /// <param name="font">The font to use.</param>
    /// <param name="fontColor">The color to draw the text in.</param>
    /// <param name="text">The text to draw.</param>
    /// <param name="YStart">The y cordinate to start drawing the first line.</param>
    /// <param name="leftBound">The left bound of the intended text space. (Text may go outside if it's too big.)</param>
    /// <param name="leftBound">The right bound of the intended text space. (Text may go outside if it's too big.)</param>
    /// <param name="lineSpacing">The hight of a line. -1 uses default font height</param>
    /// <returns>The total height of the text.</returns>
    public static int drawCenteredString(SpriteBatch spriteBatch, SpriteFont font, Color fontColor, string text, float YStart, float leftBound, float rightBound, int lineSpacing = -1)
    {
        return drawCenteredString(spriteBatch, font, fontColor, text, YStart, (rightBound - leftBound) / 2, lineSpacing);
    }

    /// <summary>Draw a centered text line.</summary>
    /// <param name="font">The font to use.</param>
    /// <param name="fontColor">The color to draw the text in.</param>
    /// <param name="text">The text to draw.</param>
    /// <param name="centre">The top centre of where the string should be drawn.</param>
    /// <param name="lineSpacing">The hight of a line. -1 uses default font height</param>
    /// <returns>The total height of the text.</returns>
    public static int drawCenteredString(SpriteBatch spriteBatch, SpriteFont font, Color color, string text, Vector2 centre, int lineSpacing = -1)
    {
        return drawCenteredString(spriteBatch, font, color, text, centre.Y, centre.X, lineSpacing);
    }

    /// <summary>Draw a centered text line.</summary>
    /// <param name="font">The font to use.</param>
    /// <param name="fontColor">The color to draw the text in.</param>
    /// <param name="text">An array with the lines to draw.</param>
    /// <param name="YStart">The y cordinate to start drawing the first line.</param>
    /// <param name="Xcentre">The x cordinate to centre the text arround.</param>
    /// <param name="lineSpacing">The hight of a line. -1 uses default font height</param>
    /// <returns>The total height of the text.</returns>
    public static int drawCenteredString(SpriteBatch spriteBatch, SpriteFont font, Color fontColor, string[] text, float YStart, float Xcentre, int lineSpacing = -1)
    {
        // temporary spacing
        int originalLineSpacing = font.LineSpacing;
        if (lineSpacing != -1) { font.LineSpacing = lineSpacing; }

        // string drawing
        for (int i = 0; i < text.Length; i++)
        {
            Vector2 stringSize = font.MeasureString(text[i]);
            spriteBatch.DrawString(font, text[i], new Vector2(Xcentre, YStart + i * font.LineSpacing), fontColor, 0, new Vector2(stringSize.X / 2, 0), 1, SpriteEffects.None, 0);
        }

        // return to normal spacing
        if (lineSpacing != -1) { font.LineSpacing = originalLineSpacing; }
        return (lineSpacing != -1 ? lineSpacing : originalLineSpacing) * text.Length;
    }

    /// <summary>Draw a centered text line.</summary>
    /// <param name="font">The font to use.</param>
    /// <param name="fontColor">The color to draw the text in.</param>
    /// <param name="text">An array with the lines to draw.</param>
    /// <param name="textBox">A rectangle specifying the place the text should be drawn. (The text may go outside this box if it doesn't fit.)</param>
    /// <param name="lineSpacing">The hight of a line. -1 uses default font height</param>
    /// <returns>The total height of the text.</returns>
    public static int drawCenteredString(SpriteBatch spriteBatch, SpriteFont font, Color fontColor, string[] text, Rectangle textBox, int lineSpacing = -1)
    {
        return drawCenteredString(spriteBatch, font, fontColor, text, textBox.Top, textBox.Left + textBox.Width / 2, lineSpacing);
    }

    /// <summary>Draw a centered text line.</summary>
    /// <param name="font">The font to use.</param>
    /// <param name="fontColor">The color to draw the text in.</param>
    /// <param name="text">An array with the lines to draw.</param>
    /// <param name="YStart">The y cordinate to start drawing the first line.</param>
    /// <param name="leftBound">The left bound of the intended text space. (Text may go outside if it's too big.)</param>
    /// <param name="leftBound">The right bound of the intended text space. (Text may go outside if it's too big.)</param>
    /// <param name="lineSpacing">The hight of a line. -1 uses default font height</param>
    /// <returns>The total height of the text.</returns>
    public static int drawCenteredString(SpriteBatch spriteBatch, SpriteFont font, Color fontColor, string[] text, float YStart, float leftBound, float rightBound, int lineSpacing = -1)
    {
        return drawCenteredString(spriteBatch, font, fontColor, text, YStart, (rightBound - leftBound) / 2, lineSpacing);
    }

    /// <summary>Draw a centered text line.</summary>
    /// <param name="font">The font to use.</param>
    /// <param name="fontColor">The color to draw the text in.</param>
    /// <param name="text">An array with the lines to draw.</param>
    /// <param name="centre">The top centre of where the string should be drawn.</param>
    /// <param name="lineSpacing">The hight of a line. -1 uses default font height</param>
    /// <returns>The total height of the text.</returns>
    public static int drawCenteredString(SpriteBatch spriteBatch, SpriteFont font, Color color, string[] text, Vector2 centre, int lineSpacing = -1)
    {
        return drawCenteredString(spriteBatch, font, color, text, centre.Y, centre.X, lineSpacing);
    }
}
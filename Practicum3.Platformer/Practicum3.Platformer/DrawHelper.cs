using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Practicum3.Platformer
{
    class DrawHelper
    {
        
        public static int drawCenteredString(SpriteBatch spriteBatch, SpriteFont font, Color fontColor, string text, float YStart, float Xcentre, int lineSpacing = -1)
        {
            if (!text.Contains("\n"))
            {
                int originalLineSpacing = font.LineSpacing;
                if (lineSpacing != -1) { font.LineSpacing = lineSpacing; }
                Vector2 stringSize = font.MeasureString(text);
                spriteBatch.DrawString(font, text, new Vector2(Xcentre, YStart), fontColor, 0, new Vector2(stringSize.X / 2, 0), 1, SpriteEffects.None, 0);

                if (lineSpacing != -1) { font.LineSpacing = originalLineSpacing; }
                return (int)stringSize.Y;
            }
            else
            {
                return drawCenteredString(spriteBatch, font, fontColor, text.Split(new string[] { "\n" }, StringSplitOptions.None), YStart, Xcentre, lineSpacing);
            }
        }

        public static int drawCenteredString(SpriteBatch spriteBatch, SpriteFont font, Color fontColor, string text, Rectangle textBox, int lineSpacing = -1)
        {
            return drawCenteredString(spriteBatch, font, fontColor, text, textBox.Top, textBox.Left + textBox.Width / 2, lineSpacing);
        }

        public static int drawCenteredString(SpriteBatch spriteBatch, SpriteFont font, Color fontColor, string text, float YStart, float leftBound, float rightBound, int lineSpacing = -1)
        {
            return drawCenteredString(spriteBatch, font, fontColor, text, YStart, (rightBound - leftBound) / 2, lineSpacing);
        }

        /// <summary>Draw a centered line.</summary>
        /// <param name="font">The font to use.</param>
        /// <param name="fontColor">The color to draw the text in.</param>
        /// <param name="text">An array with the lines to draw.</param>
        /// <param name="YStart">The y cordinate to start drawing the first line.</param>
        /// <param name="centre">The x cordinate to centre the text arround.</param>
        /// <param name="lineSpacing">The hight of a line. -1 uses default font height</param>
        /// <returns>The total height of the text.</returns>
        public static int drawCenteredString(SpriteBatch spriteBatch, SpriteFont font, Color fontColor, string[] text, float YStart, float centre, int lineSpacing = -1)
        {
            int originalLineSpacing = font.LineSpacing;
            if (lineSpacing != -1) { font.LineSpacing = lineSpacing; }

            for (int i = 0; i < text.Length; i++)
            {
                Vector2 stringSize = font.MeasureString(text[i]);
                spriteBatch.DrawString(font, text[i], new Vector2(centre, YStart + i * font.LineSpacing), fontColor, 0, new Vector2(stringSize.X / 2, 0), 1, SpriteEffects.None, 0);
            }

            if (lineSpacing != -1) { font.LineSpacing = originalLineSpacing; }
            return lineSpacing * text.Length;
        }

        public static int drawCenteredString(SpriteBatch spriteBatch, SpriteFont font, Color fontColor, string[] text, Rectangle textBox, int lineSpacing = -1)
        {
            return drawCenteredString(spriteBatch, font, fontColor, text, textBox.Top, textBox.Left + textBox.Width / 2, lineSpacing);
        }

        public static int drawCenteredString(SpriteBatch spriteBatch, SpriteFont font, Color fontColor, string[] text, float YStart, float leftBound, float rightBound, int lineSpacing = -1)
        {
            return drawCenteredString(spriteBatch, font, fontColor, text, YStart, (rightBound - leftBound) / 2, lineSpacing);
        }

    }
}

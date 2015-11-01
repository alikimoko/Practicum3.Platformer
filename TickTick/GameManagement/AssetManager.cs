using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

public class AssetManager
{
    protected ContentManager contentManager;

    /// <summary>Initialize the asset manager.</summary>
    public AssetManager(ContentManager Content)
    { contentManager = Content; }

    /// <summary>Get a sprite.</summary>
    /// <param name="assetName">The name of the sprite to load.</param>
    public Texture2D GetSprite(string assetName)
    {
        if (assetName == "")
            return null;

        try
        { return contentManager.Load<Texture2D>(assetName); }
        catch
        {
            // sprite with this name does not exist
            System.Diagnostics.Debug.WriteLine("Sprite with name \"" + assetName + "\" does not exist.");
            return null;
        }
    }

    public void PlaySound(string assetName)
    {
        try
        {
            SoundEffect snd = contentManager.Load<SoundEffect>(assetName);
            snd.Play();
        }
        catch
        {
            // sound effect with this name does not exist
            System.Diagnostics.Debug.WriteLine("Sound effect with name \"" + assetName + "\" does not exist or is not a valid filetype.");
        }
    }

    public void PlayMusic(string assetName, bool repeat = true)
    {
        MediaPlayer.IsRepeating = repeat;
        try
        {
            MediaPlayer.Play(contentManager.Load<Song>(assetName));
        }
        catch
        {
            // song with this name does not exist
            System.Diagnostics.Debug.WriteLine("Song with name \"" + assetName + "\" does not exist or is not a valid filetype.");
        }
    }

    /// <summary>Get the current content manager.</summary>
    public ContentManager Content
    { get { return contentManager; } }
}
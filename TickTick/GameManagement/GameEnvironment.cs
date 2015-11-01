using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class GameEnvironment : Game
{
    protected GraphicsDeviceManager graphics;
    protected SpriteBatch spriteBatch;
    protected InputHelper inputHelper;
    protected Matrix spriteScale;

    protected static Point screen;
    protected static GameStateManager gameStateManager;
    protected static Random random;
    protected static AssetManager assetManager;
    protected static GameSettingsManager gameSettingsManager;
    protected static Camera activeCamera;

    /// <summary>Create a new game with basic environement methods and variables.</summary>
    public GameEnvironment()
    {
        graphics = new GraphicsDeviceManager(this);

        inputHelper = new InputHelper();
        gameStateManager = new GameStateManager();
        spriteScale = Matrix.CreateScale(1, 1, 1);
        random = new Random();
        assetManager = new AssetManager(Content);
        gameSettingsManager = new GameSettingsManager();
    }

    /// <summary>The current screen size.</summary>
    public static Point Screen
    {
        get { return screen; }
        set { screen = value; }
    }

    /// <summary>Get the current random.</summary>
    public static Random Random
    { get { return random; } }

    /// <summary>Get the current asset manager.</summary>
    public static AssetManager AssetManager
    { get { return assetManager; } }

    /// <summary>Get the current game state manager.</summary>
    public static GameStateManager GameStateManager
    { get { return gameStateManager; } }

    /// <summary>Get the current game settings manager.</summary>
    public static GameSettingsManager GameSettingsManager
    { get { return gameSettingsManager; } }

    /// <summary>The current camera.</summary>
    public static Camera ActiveCamera
    {
        get { return activeCamera; }
        set { activeCamera = value; }
    }

    /// <summary>Set fullscreen mode.</summary>
    /// <param name="fullscreen">Should the fullscreen mode be activated?</param>
    public void SetFullScreen(bool fullscreen = true)
    {
        // calculate current scales
        float scalex = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / (float)screen.X;
        float scaley = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / (float)screen.Y;
        float finalscale = 1f;

        // calculate scales after fullscreen mode change
        if (!fullscreen)
        {
            if (scalex < 1f || scaley < 1f)
                finalscale = Math.Min(scalex, scaley);
        }
        else
        {
            finalscale = scalex;
            if (Math.Abs(1 - scaley) < Math.Abs(1 - scalex))
                finalscale = scaley;
        }

        // apply new mode
        graphics.PreferredBackBufferWidth = (int)(finalscale * screen.X);
        graphics.PreferredBackBufferHeight = (int)(finalscale * screen.Y);
        graphics.IsFullScreen = fullscreen;
        graphics.ApplyChanges();
        inputHelper.Scale = new Vector2((float)GraphicsDevice.Viewport.Width / screen.X,
                                        (float)GraphicsDevice.Viewport.Height / screen.Y);
        spriteScale = Matrix.CreateScale(inputHelper.Scale.X, inputHelper.Scale.Y, 1);
    }

    protected override void LoadContent()
    {
        DrawingHelper.Initialize(GraphicsDevice);
        spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    /// <summary>Handle user inputs.</summary>
    protected void HandleInput()
    {
        inputHelper.Update();
        if (inputHelper.KeyPressed(Keys.Escape))
            // exit the game
            Exit();
        if (inputHelper.KeyPressed(Keys.F5))
            // toggle fullscreen mode
            SetFullScreen(!graphics.IsFullScreen);

        // handle inputs for the current game state
        gameStateManager.HandleInput(inputHelper);
    }

    /// <summary>Update the game.</summary>
    protected override void Update(GameTime gameTime)
    {
        HandleInput();
        // handle updates for the current game state
        gameStateManager.Update(gameTime);
    }

    /// <summary>Draw the game.</summary>
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        // apply scaling
        spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, spriteScale);
        // draw the current game state
        gameStateManager.Draw(gameTime, spriteBatch);
        spriteBatch.End();
    }
}
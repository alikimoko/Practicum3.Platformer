using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class GameOverState : GameObjectList
{
    protected IGameLoopObject playingState;

    /// <summary>Create the game over state.</summary>
    public GameOverState()
    {
        playingState = GameEnvironment.GameStateManager.GetGameState("playingState");
        SpriteGameObject overlay = new SpriteGameObject("Overlays/spr_gameover");
        overlay.Position = new Vector2(GameEnvironment.Screen.X, GameEnvironment.Screen.Y) / 2 - overlay.Center;
        Add(overlay);
    }

    /// <summary>Enables retrying the level.</summary>
    public override void HandleInput(InputHelper inputHelper)
    {
        if (!(inputHelper.KeyPressed(Keys.Space) || inputHelper.ControlerButtonPressed(Buttons.A)))
            return;
        playingState.Reset();
        GameEnvironment.GameStateManager.SwitchTo("playingState");
    }

    /// <summary>Update the level in the background.</summary>
    public override void Update(GameTime gameTime)
    { playingState.Update(gameTime); }

    /// <summary>Draw the mesage.</summary>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        playingState.Draw(gameTime, spriteBatch);
        base.Draw(gameTime, spriteBatch);
    }
}
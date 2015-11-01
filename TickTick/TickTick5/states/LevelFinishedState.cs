using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class LevelFinishedState : GameObjectList
{
    protected IGameLoopObject playingState;

    /// <summary>Create the level finished state.</summary>
    public LevelFinishedState()
    {
        playingState = GameEnvironment.GameStateManager.GetGameState("playingState");
        SpriteGameObject overlay = new SpriteGameObject("Overlays/spr_welldone");
        overlay.Position = new Vector2(GameEnvironment.Screen.X, GameEnvironment.Screen.Y) / 2 - overlay.Center;
        Add(overlay);
    }

    /// <summary>Enables going to the next level.</summary>
    public override void HandleInput(InputHelper inputHelper)
    {
        if (!(inputHelper.KeyPressed(Keys.Space) || inputHelper.ControlerButtonPressed(Buttons.A)))
            return;
        GameEnvironment.GameStateManager.SwitchTo("playingState");
        (playingState as PlayingState).NextLevel();
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
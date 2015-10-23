using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class GameStateManager : IGameLoopObject
{
    Dictionary<string, IGameLoopObject> gameStates;
    IGameLoopObject currentGameState;

    /// <summary>Create a new game state manager.</summary>
    public GameStateManager()
    {
        gameStates = new Dictionary<string, IGameLoopObject>();
        currentGameState = null;
    }

    /// <summary>Add a new game state.</summary>
    /// <param name="name">The name of the game state.</param>
    /// <param name="state">The game state to be executed.</param>
    public void AddGameState(string name, IGameLoopObject state)
    { gameStates[name] = state; }

    /// <summary>Get a given game state.</summary>
    /// <param name="name">The name of the game state.</param>
    public IGameLoopObject GetGameState(string name)
    { return gameStates[name]; }

    /// <summary>Switch to a given game state</summary>
    /// <param name="name">The name of the game state to switch to.</param>
    public void SwitchTo(string name)
    {
        if (gameStates.ContainsKey(name))
            currentGameState = gameStates[name];
        else
            // hard error
            throw new KeyNotFoundException("Could not find game state \"" + name + "\"");
    }

    /// <summary>Get the current game state.</summary>
    public IGameLoopObject CurrentGameState
    { get { return currentGameState; } }

    /// <summary>Handle the input for the current game state.</summary>
    public void HandleInput(InputHelper inputHelper)
    {
        if (currentGameState != null)
            currentGameState.HandleInput(inputHelper);
    }

    /// <summary>Update the current game state.</summary>
    public void Update(GameTime gameTime)
    {
        if (currentGameState != null)
            currentGameState.Update(gameTime);
    }

    /// <summary>Draw the current game state.</summary>
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (currentGameState != null)
            currentGameState.Draw(gameTime, spriteBatch);
    }

    /// <summary>Reset the current game state.</summary>
    public void Reset()
    {
        if (currentGameState != null)
            currentGameState.Reset();
    }
}

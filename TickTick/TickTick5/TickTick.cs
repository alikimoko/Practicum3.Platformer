using Microsoft.Xna.Framework;

class TickTick : GameEnvironment
{
    /// <summary>Start the game.</summary>
    static void Main()
    {
        TickTick game = new TickTick();
        game.Run();
    }

    /// <summary>Create the game.</summary>
    public TickTick()
    {
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    /// <summary>Load the gamstates and background music.</summary>
    protected override void LoadContent()
    {
        base.LoadContent();

        screen = new Point(1440, 825);
        SetFullScreen(false);

        gameStateManager.AddGameState("titleMenu", new TitleMenuState());
        gameStateManager.AddGameState("helpState", new HelpState());
        gameStateManager.AddGameState("playingState", new PlayingState(Content));
        gameStateManager.AddGameState("levelMenu", new LevelMenuState());
        gameStateManager.AddGameState("gameOverState", new GameOverState());
        gameStateManager.AddGameState("levelFinishedState", new LevelFinishedState());
        gameStateManager.SwitchTo("titleMenu");

        AssetManager.PlayMusic("Sounds/snd_music");
    }
}
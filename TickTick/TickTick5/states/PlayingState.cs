using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

class PlayingState : IGameLoopObject
{
    protected List<Level> levels;
    protected int currentLevelIndex;
    protected ContentManager Content;

    /// <summary>Create the playing state.</summary>
    public PlayingState(ContentManager Content)
    {
        this.Content = Content;
        currentLevelIndex = -1;
        levels = new List<Level>();
        LoadLevels();
        LoadLevelsStatus(Content.RootDirectory + "/Levels/levels_status.txt");
    }

    /// <summary>Get the current level.</summary>
    public Level CurrentLevel
    { get { return levels[currentLevelIndex]; } }

    /// <summary>The index of the current level.</summary>
    public int CurrentLevelIndex
    {
        get { return currentLevelIndex; }
        set
        {
            if (value >= 0 && value < levels.Count)
            {
                currentLevelIndex = value;
                CurrentLevel.Reset();
            }
        }
    }

    /// <summary>Get the level list.</summary>
    public List<Level> Levels
    { get { return levels; } }

    /// <summary>Handle the input for the current level.</summary>
    public virtual void HandleInput(InputHelper inputHelper)
    { CurrentLevel.HandleInput(inputHelper); }

    /// <summary>Update the current level.</summary>
    public virtual void Update(GameTime gameTime)
    {
        CurrentLevel.Update(gameTime);
        if (CurrentLevel.GameOver)
            GameEnvironment.GameStateManager.SwitchTo("gameOverState");
        else if (CurrentLevel.Completed)
        {
            CurrentLevel.Solved = true;
            GameEnvironment.GameStateManager.SwitchTo("levelFinishedState");
        }
    }

    /// <summary>Draw the current level.</summary>
    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    { CurrentLevel.Draw(gameTime, spriteBatch); }

    /// <summary>Reset the current level.</summary>
    public virtual void Reset()
    { CurrentLevel.Reset(); }

    /// <summary>Go to the next level.</summary>
    public void NextLevel()
    {
        CurrentLevel.Reset();
        if (currentLevelIndex >= levels.Count - 1)
            GameEnvironment.GameStateManager.SwitchTo("levelMenu");
        else
        {
            CurrentLevelIndex++;
            levels[currentLevelIndex].Locked = false;
            GameEnvironment.ActiveCamera = levels[currentLevelIndex].LevelCamera;
        }
        WriteLevelsStatus(Content.RootDirectory + "/Levels/levels_status.txt");
    }

    /// <summary>Load all levels.</summary>
    public void LoadLevels()
    {
        for (int currLevel = 1; currLevel <= 11; currLevel++)
            levels.Add(new Level(currLevel));
    }

    /// <summary>Load the level status.</summary>
    /// <param name="path">The path to the level status file.</param>
    public void LoadLevelsStatus(string path)
    {
        List<string> textlines = new List<string>();
        StreamReader fileReader = new StreamReader(path);
        for (int i = 0; i < levels.Count; i++)
        {
            string line = fileReader.ReadLine();
            string[] elems = line.Split(',');
            if (elems.Length == 2)
            {
                levels[i].Locked = bool.Parse(elems[0]);
                levels[i].Solved = bool.Parse(elems[1]);
            }
        }
        fileReader.Close();
    }

    /// <summary>Update the level status.</summary>
    /// <param name="path">The path to the level status file.</param>
    public void WriteLevelsStatus(string path)
    {
        // write the lines
        List<string> textlines = new List<string>();
        StreamWriter fileWriter = new StreamWriter(path, false);
        for (int i = 0; i < levels.Count; i++)
        {
            string line = levels[i].Locked.ToString() + "," + levels[i].Solved.ToString();
            fileWriter.WriteLine(line);
        }
        fileWriter.Close();
    }
}
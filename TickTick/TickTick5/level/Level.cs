using System.Collections.Generic;
using Microsoft.Xna.Framework;

partial class Level : GameObjectList
{
    protected bool locked, solved;
    protected Button quitButton;
    protected Camera levelCamera;
    protected int height, width;

    protected List<Projectile> projectiles = new List<Projectile>();

    /// <summary>Create the level.</summary>
    /// <param name="levelIndex">The index of the level.</param>
    public Level(int levelIndex)
    {
        // load the backgrounds
        GameObjectList backgrounds = new GameObjectList(0, "backgrounds");
        SpriteGameObject background_main = new SpriteGameObject("Backgrounds/spr_sky");
        background_main.Position = new Vector2(0, GameEnvironment.Screen.Y - background_main.Height);
        backgrounds.Add(background_main);

        Add(new GameObjectList(1, "waterdrops"));
        Add(new GameObjectList(2, "enemies"));

        LoadTiles("Content/Levels/" + levelIndex + ".txt");

        // add a few random mountains
        for (int i = 0; i < 5; i++)
        {
            SpriteGameObject mountain = new SpriteGameObject("Backgrounds/spr_mountain_" + (GameEnvironment.Random.Next(2) + 1), 1, "", 0, SpriteGameObject.Backgroundlayer.background);
            mountain.Position = new Vector2((float)GameEnvironment.Random.NextDouble() * (width * cellWidth) - mountain.Width / 2,
                (height * cellHeight) - mountain.Height);
            backgrounds.Add(mountain);
        }

        Clouds clouds = new Clouds(2, "", this);
        backgrounds.Add(clouds);
        Add(backgrounds);

        SpriteGameObject timerBackground = new SpriteGameObject("Sprites/spr_timer", 100);
        timerBackground.Position = new Vector2(10, 10);
        Add(timerBackground);
        
        quitButton = new Button("Sprites/spr_button_quit", 100);
        quitButton.Position = new Vector2(GameEnvironment.Screen.X - quitButton.Width - 10, 10);
        Add(quitButton);
        
    }

    /// <summary>Has the level been completed?</summary>
    public bool Completed
    {
        get
        {
            SpriteGameObject exitObj = Find("exit") as SpriteGameObject;
            Player player = Find("player") as Player;
            if (!exitObj.CollidesWith(player))
                return false;
            GameObjectList waterdrops = Find("waterdrops") as GameObjectList;
            foreach (GameObject d in waterdrops.Objects)
                if (d.Visible)
                    return false;
            return true;
        }
    }

    /// <summary>Isthe player game over?</summary>
    public bool GameOver
    {
        get
        {
            TimerGameObject timer = Find("timer") as TimerGameObject;
            Player player = Find("player") as Player;
            return !player.IsAlive || timer.GameOver;
        }
    }

    /// <summary>Is the level locked?</summary>
    public bool Locked
    {
        get { return locked; }
        set { locked = value; }
    }

    /// <summary>Is the level solved?</summary>
    public bool Solved
    {
        get { return solved; }
        set { solved = value; }
    }

    /// <summary>Get the camera for this level.</summary>
    public Camera LevelCamera
    { get { return levelCamera; } }

    /// <summary>Get the width of the level in pixels.</summary>
    public int Width
    { get { return width * 72; } }

    /// <summary>Get the height of the level in pixels.</summary>
    public int Height
    { get { return height * 55; } }

    /// <summary>Get all current projectiles.</summary>
    public List<Projectile> Projectiles
    { get { return projectiles; } }
}


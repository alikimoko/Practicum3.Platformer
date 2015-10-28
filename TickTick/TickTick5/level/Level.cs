using Microsoft.Xna.Framework;

partial class Level : GameObjectList
{
    protected bool locked, solved;
    protected Button quitButton;
    protected Camera levelCamera;
    protected int height, width;

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
            SpriteGameObject mountain = new SpriteGameObject("Backgrounds/spr_mountain_" + (GameEnvironment.Random.Next(2) + 1), 1);
            mountain.Position = new Vector2((float)GameEnvironment.Random.NextDouble() * width - mountain.Width / 2, height - mountain.Height);
            backgrounds.Add(mountain);
        }

        Clouds clouds = new Clouds(2, "", this);
        backgrounds.Add(clouds);
        Add(backgrounds);

        SpriteGameObject timerBackground = new SpriteGameObject("Sprites/spr_timer", 100);
        timerBackground.Position = new Vector2(10, 10);
        Add(timerBackground);
        TimerGameObject timer = new TimerGameObject(101, "timer");
        timer.Position = new Vector2(25, 30);
        Add(timer);

        quitButton = new Button("Sprites/spr_button_quit", 100);
        quitButton.Position = new Vector2(GameEnvironment.Screen.X - quitButton.Width - 10, 10);
        Add(quitButton);
        
    }

    public bool Completed
    {
        get
        {
            SpriteGameObject exitObj = this.Find("exit") as SpriteGameObject;
            Player player = this.Find("player") as Player;
            if (!exitObj.CollidesWith(player))
                return false;
            GameObjectList waterdrops = this.Find("waterdrops") as GameObjectList;
            foreach (GameObject d in waterdrops.Objects)
                if (d.Visible)
                    return false;
            return true;
        }
    }

    public bool GameOver
    {
        get
        {
            TimerGameObject timer = this.Find("timer") as TimerGameObject;
            Player player = this.Find("player") as Player;
            return !player.IsAlive || timer.GameOver;
        }
    }

    public bool Locked
    {
        get { return locked; }
        set { locked = value; }
    }

    public bool Solved
    {
        get { return solved; }
        set { solved = value; }
    }

    public Camera LevelCamera
    { get { return levelCamera; } }

    public int Width
    { get { return width; } }

    public int Height
    { get { return height; } }
}


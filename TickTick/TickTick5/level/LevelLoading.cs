using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

partial class Level : GameObjectList
{
    private string path;

    /// <summary>Load the level.</summary>
    public void LoadTiles(string path)
    {
        this.path = path;
        List<string> textlines = new List<string>();
        StreamReader fileReader = new StreamReader(path);
        string line = fileReader.ReadLine(); // first row of the level
        width = line.Length; // calculated level width
        while (line != null)
        {
            // read all lines
            textlines.Add(line);
            line = fileReader.ReadLine();
        }
        height = textlines.Count - 1;

        // add the hint
        GameObjectList hintfield = new GameObjectList(100);
        Add(hintfield);
        SpriteGameObject hint_frame = new SpriteGameObject("Overlays/spr_frame_hint", 1); // hint background
        hintfield.Position = new Vector2((GameEnvironment.Screen.X - hint_frame.Width) / 2, 10);
        hintfield.Add(hint_frame);
        TextGameObject hintText = new TextGameObject("Fonts/HintFont", 2); // hint text
        hintText.Text = textlines[textlines.Count - 1]; // last line in the level descriptor
        hintText.Position = new Vector2(120, 25);
        hintText.Color = Color.Black;
        hintfield.Add(hintText);
        VisibilityTimer hintTimer = new VisibilityTimer(hintfield, 1, "hintTimer"); // timeout
        Add(hintTimer);

        // construct the level
        TileField tiles = new TileField(height, width, 1, "tiles");
        Add(tiles);
        // tile dimentions
        tiles.CellWidth = 72;
        tiles.CellHeight = 55;
        for (int x = 0; x < width; ++x)
            for (int y = 0; y < height; ++y)
            {
                Tile t = LoadTile(textlines[y][x], x, y);
                tiles.Add(t, x, y);
            }
    }

    /// <summary>Load the tile.</summary>
    /// <param name="tileType">The tile to load.</param>
    /// <param name="x">The x coördinate to place the tile.</param>
    /// <param name="y">The y coördinate to place the tile.</param>
    /// <returns>The correct tile.</returns>
    private Tile LoadTile(char tileType, int x, int y)
    {
        switch (tileType)
        {
            case '.':
                return new Tile();
            case '-':
                return LoadBasicTile("spr_platform", TileType.Platform);
            case '+':
                return LoadBasicTile("spr_platform_hot", TileType.Platform, true, false);
            case '@':
                return LoadBasicTile("spr_platform_ice", TileType.Platform, false, true);
            case 'X':
                return LoadEndTile(x, y);
            case 'W':
                return LoadWaterTile(x, y);
            case '1':
                return LoadStartTile(x, y);
            case '#':
                return LoadBasicTile("spr_wall", TileType.Normal);
            case '^':
                return LoadBasicTile("spr_wall_hot", TileType.Normal, true, false);
            case '*':
                return LoadBasicTile("spr_wall_ice", TileType.Normal, false, true);
            case 'T':
                return LoadTurtleTile(x, y);
            case 'R':
                return LoadRocketTile(x, y, true);
            case 'r':
                return LoadRocketTile(x, y, false);
            case 'S':
                return LoadSparkyTile(x, y);
            case 'A':
            case 'B':
            case 'C':
                return LoadFlameTile(x, y, tileType);
            default:
                System.Diagnostics.Debug.WriteLine("Used a non existing tile type \"" + tileType + "\" at (" + x + "," + y + ") in \"" + path.Substring(path.LastIndexOf('/') + 1) + "\"." );
                return new Tile("");
        }
    }

    /// <summary>Load a standard, hot or ice tile.</summary>
    /// <param name="name">The sprite to use.</param>
    /// <param name="tileType">Jump through or block?</param>
    /// <param name="hot">Is the tile hot?</param>
    /// <param name="ice">Is the tile ice?</param>
    private Tile LoadBasicTile(string name, TileType tileType, bool hot = false, bool ice = false)
    {
        Tile t = new Tile("Tiles/" + name, tileType);
        t.Hot = hot;
        t.Ice = ice;
        return t;
    }

    /// <summary>The player starting position and player and camera creation.</summary>
    private Tile LoadStartTile(int x, int y)
    {
        TileField tiles = Find("tiles") as TileField;
        Vector2 startPosition = new Vector2((x + 0.5f) * tiles.CellWidth, (y + 1) * tiles.CellHeight);
        Player player = new Player(startPosition);
        Add(player);

        // create the camera
        levelCamera = new Camera(GameEnvironment.Screen.X, GameEnvironment.Screen.Y,
                                 startPosition - new Vector2(GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y / 2),
                                 width * tiles.CellWidth, height * tiles.CellHeight,
                                 height * tiles.CellHeight > GameEnvironment.Screen.Y);

        levelCamera.moveCamera(player.GlobalPosition, player.Width, player.Height);

        return new Tile();
    }

    /// <summary>Add a flame enemy.</summary>
    /// <param name="enemyType">The enemy movement type.</param>
    private Tile LoadFlameTile(int x, int y, char enemyType)
    {
        GameObjectList enemies = Find("enemies") as GameObjectList;
        TileField tiles = Find("tiles") as TileField;
        GameObject enemy = null;
        switch (enemyType)
        {
            case 'A': enemy = new UnpredictableEnemy(); break;
            case 'B': enemy = new PlayerFollowingEnemy(); break;
            case 'C': 
            default:  enemy = new PatrollingEnemy(); break;
        }
        enemy.Position = new Vector2((x + 0.5f) * tiles.CellWidth, (y + 1) * tiles.CellHeight);
        enemies.Add(enemy);
        return new Tile();
    }

    /// <summary>Load a turtle enemy.</summary>
    private Tile LoadTurtleTile(int x, int y)
    {
        GameObjectList enemies = Find("enemies") as GameObjectList;
        TileField tiles = Find("tiles") as TileField;
        Turtle enemy = new Turtle();
        enemy.Position = new Vector2((x + 0.5f) * tiles.CellWidth, (y + 1) * tiles.CellHeight + 25.0f);
        enemies.Add(enemy);
        return new Tile();
    }

    /// <summary>Create a sparky enemy.</summary>
    private Tile LoadSparkyTile(int x, int y)
    {
        GameObjectList enemies = Find("enemies") as GameObjectList;
        TileField tiles = Find("tiles") as TileField;
        Sparky enemy = new Sparky((y + 1) * tiles.CellHeight - 100f);
        enemy.Position = new Vector2((x + 0.5f) * tiles.CellWidth, (y + 1) * tiles.CellHeight - 100f);
        enemies.Add(enemy);
        return new Tile();
    }

    /// <summary>Create a rocket enemy.</summary>
    /// <param name="moveToLeft">Is the rocket going to the left?</param>
    private Tile LoadRocketTile(int x, int y, bool moveToLeft)
    {
        GameObjectList enemies = Find("enemies") as GameObjectList;
        TileField tiles = Find("tiles") as TileField;
        Vector2 startPosition = new Vector2((x + 0.5f) * tiles.CellWidth, (y + 1) * tiles.CellHeight);
        Rocket enemy = new Rocket(moveToLeft, startPosition);
        enemies.Add(enemy);
        return new Tile();
    }

    /// <summary>Add the goal of the level.</summary>
    private Tile LoadEndTile(int x, int y)
    {
        TileField tiles = Find("tiles") as TileField;
        SpriteGameObject exitObj = new SpriteGameObject("Sprites/spr_goal", 1, "exit", 0, true);
        exitObj.Position = new Vector2(x * tiles.CellWidth, (y+1) * tiles.CellHeight);
        exitObj.Origin = new Vector2(0, exitObj.Height);
        Add(exitObj);
        return new Tile();
    }

    /// <summary>Add a water tile.</summary>
    private Tile LoadWaterTile(int x, int y)
    {
        GameObjectList waterdrops = Find("waterdrops") as GameObjectList;
        TileField tiles = Find("tiles") as TileField;
        WaterDrop w = new WaterDrop();
        w.Origin = w.Center;
        w.Position = new Vector2((x + 0.5f) * tiles.CellWidth, (y + 0.5f) * tiles.CellHeight - 10);
        waterdrops.Add(w);
        return new Tile();
    }
}
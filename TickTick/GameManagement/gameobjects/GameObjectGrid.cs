using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class GameObjectGrid : GameObject
{
    protected GameObject[,] grid;
    protected int cellWidth, cellHeight;

    /// <summary> Create a new game object grid.</summary>
    /// <param name="rows">The amount of rows in the grid.</param>
    /// <param name="columns">The amount of columns in the grid.</param>
    /// <param name="layer">The layer the grid is in.</param>
    /// <param name="id">The ID to refer to the grid.</param>
    public GameObjectGrid(int rows, int columns, int layer = 0, string id = "")
        : base(layer, id)
    {
        grid = new GameObject[columns, rows];
        for (int x = 0; x < columns; x++)
            for (int y = 0; y < rows; y++)
                grid[x, y] = null;
    }

    /// <summary>Add an object to the grid.</summary>
    /// <param name="obj">The object to add.</param>
    /// <param name="x">The column to add the object in.</param>
    /// <param name="y">The row to add the object in.</param>
    public void Add(GameObject obj, int x, int y)
    {
        grid[x, y] = obj;
        obj.Parent = this;
        obj.Position = new Vector2(x * cellWidth, y * cellHeight);
    }

    /// <summary>Get the object in column x and row y.</summary>
    public GameObject Get(int x, int y)
    {
        if (x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1))
            return grid[x, y];
        else
            return null;
    }

    /// <summary>Get this object its grid.</summary>
    public GameObject[,] Objects
    { get { return grid; } }

    /// <summary>Get the internal position of a given object.</summary>
    /// <param name="s">The object to search for.</param>
    public Vector2 GetAnchorPosition(GameObject s)
    {
        for (int x = 0; x < Columns; x++)
            for (int y = 0; y < Rows; y++)
                if (grid[x, y] == s)
                    return new Vector2(x * cellWidth, y * cellHeight);
        return Vector2.Zero;
    }

    /// <summary>Get the number of rows in this grid.</summary>
    public int Rows
    { get { return grid.GetLength(1); } }

    /// <summary>Get the number of columns in this grid.</summary>
    public int Columns
    { get { return grid.GetLength(0); } }

    /// <summary>The width of the cels in this grid.</summary>
    public int CellWidth
    {
        get { return cellWidth; }
        set { cellWidth = value; }
    }

    /// <summary>The height of the cels in this grid.</summary>
    public int CellHeight
    {
        get { return cellHeight; }
        set { cellHeight = value; }
    }

    /// <summary>Handle the inputs of the objects in this grid.</summary>
    public override void HandleInput(InputHelper inputHelper)
    {
        foreach (GameObject obj in grid)
            obj.HandleInput(inputHelper);
    }

    /// <summary>Update the objects in this grid.</summary>
    public override void Update(GameTime gameTime)
    {
        foreach (GameObject obj in grid)
            obj.Update(gameTime);
    }

    /// <summary>Draw the objects in this grid.</summary>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        foreach (GameObject obj in grid)
            obj.Draw(gameTime, spriteBatch);
    }

    /// <summary>Reset the grid and the objects in this grid.</summary>
    public override void Reset()
    {
        base.Reset();
        foreach (GameObject obj in grid)
            obj.Reset();
    }
}

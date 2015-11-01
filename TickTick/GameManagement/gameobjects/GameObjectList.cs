using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class GameObjectList : GameObject
{
    protected List<GameObject> gameObjects;

    /// <summary>Create a new game object list.</summary>
    public GameObjectList(int layer = 0, string id = "") : base(layer, id)
    { gameObjects = new List<GameObject>(); }

    /// <summary>Add the given object to the list.</summary>
    public void Add(GameObject obj)
    {
        obj.Parent = this;
        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (gameObjects[i].Layer > obj.Layer)
            {
                gameObjects.Insert(i, obj);
                return;
            }
        }
        gameObjects.Add(obj);
    }

    /// <summary>Remove the given object from the list.</summary>
    public void Remove(GameObject obj)
    {
        gameObjects.Remove(obj);
        obj.Parent = null;
    }

    /// <summary>Find the object with the given ID.</summary>
    public GameObject Find(string id)
    {
        foreach (GameObject obj in gameObjects)
        {
            if (obj.ID == id)
                return obj;
            if (obj is GameObjectList)
            {
                GameObjectList objlist = obj as GameObjectList;
                GameObject subobj = objlist.Find(id);
                if (subobj != null)
                    return subobj;
            }
        }
        return null;
    }

    /// <summary>Get the object list.</summary>
    public List<GameObject> Objects
    { get { return gameObjects; } }

    /// <summary>Handle the inputs for the objects in the list.</summary>
    public override void HandleInput(InputHelper inputHelper)
    {
        for (int i = gameObjects.Count - 1; i >= 0; i--)
            gameObjects[i].HandleInput(inputHelper);
    }

    /// <summary>Update the objects in the list.</summary>
    public override void Update(GameTime gameTime)
    {
        for (int i = gameObjects.Count - 1; i >= 0; i--)
            gameObjects[i].Update(gameTime);
    }

    /// <summary>Draw the objects in the list.</summary>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (!visible)
            return;
        List<GameObject>.Enumerator e = gameObjects.GetEnumerator();        
        while (e.MoveNext())
            e.Current.Draw(gameTime, spriteBatch);
    }

    /// <summary>Reset the objects in the list.</summary>
    public override void Reset()
    {
        base.Reset();
        foreach (GameObject obj in gameObjects)
            obj.Reset();
    }
}

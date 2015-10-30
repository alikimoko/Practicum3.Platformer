using Microsoft.Xna.Framework;

public class Camera
{
    Vector2 position;
    Rectangle cameraOutline, movingRectangle;
    bool moveHorizontal, moveVertical;
    int moveableWidth, moveableHeight;
    int rightBoundary, lowerBoundary;

    /// <summary>Create a new camera. NOTE: call moveCamera directly after this to align the camera</summary>
    /// <param name="levelWidth"></param>
    /// <param name="levelHeigth"></param>
    /// <param name="moveableHeight"></param>
    /// <param name="moveableWidth"></param>
    public Camera(int levelWidth, int levelHeigth, int moveableHeight = 200, int moveableWidth = 200)
    {
        moveHorizontal = levelWidth > GameEnvironment.Screen.X;
        moveVertical = levelHeigth > GameEnvironment.Screen.Y;
        this.moveableWidth = moveableWidth;
        this.moveableHeight = moveableHeight;

        cameraOutline = new Rectangle(0, 0, GameEnvironment.Screen.X, GameEnvironment.Screen.Y);
        movingRectangle = new Rectangle((cameraOutline.Width - moveableWidth) / 2, (cameraOutline.Height - moveableHeight) / 2, moveableWidth, moveableHeight);
        calculateBoundary(levelWidth, levelHeigth);
    }

    private void calculateBoundary(int levelWidth, int levelHeigth)
    {
        rightBoundary = levelWidth - cameraOutline.Width;
        lowerBoundary = levelHeigth - cameraOutline.Height;
    }

    public bool MoveHorizontal { get { return moveHorizontal; } }
    public bool MoveVertical { get { return moveVertical; } }

    public Rectangle CameraOutline { get { return cameraOutline; } }

    public Vector2 Position
    {
        get { return position; }
        set
        {
            Vector2 tempVector = new Vector2(MathHelper.Clamp(value.X, 0, rightBoundary), MathHelper.Clamp(value.Y, 0, lowerBoundary));
            
            if (moveHorizontal && tempVector.X != position.X)
            {
                position.X = tempVector.X;
                cameraOutline.X = (int)tempVector.X;
                movingRectangle.X = cameraOutline.Center.X - moveableWidth / 2;
            }
            if (moveVertical && tempVector.Y != position.Y)
            {
                position.Y = tempVector.Y;
                cameraOutline.Y = (int)tempVector.Y;
                movingRectangle.Y = cameraOutline.Center.Y - moveableHeight / 2;
            }
        }
    }

    public void moveCamera(Vector2 centre)
    {
        Vector2 tempVector = position;
        bool change = false;

        if (moveHorizontal)
        {
            if (centre.X < movingRectangle.X)
            {
                tempVector.X -= (movingRectangle.X - centre.X);
                change = true;
            }
            else if (centre.X > movingRectangle.Right)
            {
                tempVector.X += (centre.X - movingRectangle.Right);
                change = true;
            }
        }

        if (moveVertical)
        {
            if (centre.Y < movingRectangle.Y)
            {
                tempVector.Y -= (movingRectangle.Y - centre.Y);
                change = true;
            }
            else if (centre.Y > movingRectangle.Bottom)
            {
                tempVector.Y += (centre.Y - movingRectangle.Bottom);
                change = true;
            }
        }
        
        if (change)
        { Position = tempVector; }
    }
}
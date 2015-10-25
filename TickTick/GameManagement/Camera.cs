using Microsoft.Xna.Framework;

class Camera
{
    Vector2 position;
    Rectangle cameraOutline, movingRectangle;
    bool moveVertical;
    int moveableWidth = 200, moveableHeight = 200; //placeholder, subject to change
    int rightBoundary, lowerBoundary;

    public Camera(int screenWidth, int screenHeight, Vector2 startingPosition, int levelWidth, int levelHeigth, bool moveVertical = false)
    {
        this.moveVertical = moveVertical;
        position = startingPosition;
        calculateBoundary(levelWidth, levelHeigth);

        cameraOutline = new Rectangle((int)position.X, (int)position.Y, screenWidth, screenHeight);
        movingRectangle = new Rectangle((int)(position.X + (cameraOutline.Width / 2 - moveableWidth / 2)), (int)(position.Y + (cameraOutline.Height / 2 - moveableHeight / 2)), moveableWidth, moveableHeight);
    }

    public Camera(int screenWidth, int screenHeight, Vector2 startingPosition, int levelWidth, int levelHeigth, int moveableHeight, int moveableWidth, bool moveVertical = false)
    {
        this.moveVertical = moveVertical;
        position = startingPosition;
        this.moveableWidth = moveableWidth;
        this.moveableHeight = moveableHeight;

        cameraOutline = new Rectangle((int)position.X, (int)position.Y, screenWidth, screenHeight);
        movingRectangle = new Rectangle((int)(position.X + (cameraOutline.Width / 2 - moveableWidth / 2)), (int)(position.Y + (cameraOutline.Height / 2 - moveableHeight / 2)), this.moveableWidth, this.moveableHeight);
    }

    private void calculateBoundary(int levelWidth, int levelHeigth)
    {
        rightBoundary = levelWidth - cameraOutline.Width;
        lowerBoundary = levelHeigth - cameraOutline.Height;
    }

    public bool MoveVertical { get { return moveVertical; } }
    public Rectangle CameraOutline { get { return cameraOutline; } }
    public Vector2 Position
    {
        get { return position; }
        set
        {
            if (value.X >= 0 && value.X <= rightBoundary)
            {
                position.X = value.X;
                cameraOutline.X = (int)value.X;
                movingRectangle.X = (int)(position.X + (cameraOutline.Width / 2 - moveableWidth / 2));
            }
            if (MoveVertical && value.Y >= 0 && value.Y <= lowerBoundary)
            {
                position.Y = value.Y;
                cameraOutline.Y = (int)value.Y;
                movingRectangle.Y = (int)(position.Y + (cameraOutline.Height / 2 - moveableHeight / 2));
            }
        }
    }

    public void moveCamera(Vector2 playerPosition, float playerWidth, float playerHeigth)
    {
        Vector2 tempVector = new Vector2(0, 0);
        bool change = false;
        if (playerPosition.X < movingRectangle.X)
        {
            tempVector.X = (position.X - (movingRectangle.X - playerPosition.X));
            change = true;
        }
        else if (playerPosition.X + playerWidth > movingRectangle.X + movingRectangle.Width)
        {
            tempVector.X = (position.X + ((playerPosition.X + playerWidth) - (movingRectangle.X + movingRectangle.Width)));
            change = true;
        }

        if (playerPosition.Y < movingRectangle.Y)
        {
            tempVector.Y = (position.Y - (movingRectangle.Y - playerPosition.Y));
            change = true;
        }
        else if (playerPosition.Y + playerHeigth > movingRectangle.Y + movingRectangle.Height)
        {
            tempVector.Y = (position.Y + ((playerPosition.Y + playerHeigth) - (movingRectangle.Y + movingRectangle.Height)));
            change = true;
        }

        if (change) { Position = tempVector; }
    }
}
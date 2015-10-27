using Microsoft.Xna.Framework;

class Rocket : AnimatedGameObject
{
    protected double spawnTime;
    protected double realSpawntime;
    protected Vector2 startPosition;
    
    /* ORIGINAL try
    public Rocket(bool moveToLeft, Vector2 startPosition)
    {
        LoadAnimation("Sprites/Rocket/spr_rocket@3", "default", true, 0.2f);
        PlayAnimation("default");
        Mirror = moveToLeft;
        this.startPosition = startPosition;
        Reset();
    }
    */

    public Rocket(bool moveToLeft, Vector2 startPosition, int extraSpawnTime = 0)
    {
        LoadAnimation("Sprites/Rocket/spr_rocket@3", "default", true, 0.2f);
        PlayAnimation("default");
        Mirror = moveToLeft;
        this.startPosition = startPosition;
        spawnTime = 2 + extraSpawnTime;   //try
        realSpawntime = spawnTime; // try
        Reset();
    }

    public override void Reset()
    {
        Visible = false;
        position = startPosition;
        velocity = Vector2.Zero;
        spawnTime = realSpawntime; //try
        //spawnTime = GameEnvironment.Random.NextDouble() * 5; try
        
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (spawnTime > 0)
        {
            spawnTime -= gameTime.ElapsedGameTime.TotalSeconds;
            return;
        }
        Visible = true;
        velocity.X = Mirror ? -600 : 600;
        CheckPlayerCollision();
        // check if we are outside the screen
        Rectangle screenBox = new Rectangle(0, 0, GameEnvironment.Screen.X, GameEnvironment.Screen.Y);
        if (!screenBox.Intersects(BoundingBox))
            Reset();
    }

    public void CheckPlayerCollision()
    {
        Player player = GameWorld.Find("player") as Player;
        if (CollidesWith(player))
            player.Die(false);
    }
}

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

partial class Player : AnimatedGameObject
{
    protected Vector2 startPosition;
    protected bool isOnTheGround;
    protected float previousYPosition;
    protected bool isAlive;
    protected bool exploded;
    protected bool finished;
    protected bool walkingOnIce, walkingOnHot;

    protected List<Projectile> projectiles = new List<Projectile>();
    
    public Player(Vector2 start) : base(2, "player", true)
    {
        LoadAnimation("Sprites/Player/spr_idle", "idle", true, 0.1f); 
        LoadAnimation("Sprites/Player/spr_run@13", "run", true, 0.05f);
        LoadAnimation("Sprites/Player/spr_jump@14", "jump", false, 0.05f); 
        LoadAnimation("Sprites/Player/spr_celebrate@14", "celebrate", false, 0.05f);
        LoadAnimation("Sprites/Player/spr_die@5", "die", false, 0.1f);
        LoadAnimation("Sprites/Player/spr_explode@5x5", "explode", false, 0.04f);

        startPosition = start;
        Reset();
    }

    public override void Reset()
    {
        position = startPosition;
        velocity = Vector2.Zero;
        isOnTheGround = true;
        isAlive = true;
        exploded = false;
        finished = false;
        walkingOnIce = false;
        walkingOnHot = false;
        PlayAnimation("idle");
        previousYPosition = BoundingBox.Bottom;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        float walkingSpeed = 400;
        if (walkingOnIce)
            walkingSpeed *= 1.5f;
        if (!isAlive)
            return;
        if (inputHelper.IsKeyDown(Keys.Left) || inputHelper.IsKeyDown(Keys.A) || inputHelper.IsControlerButtonDown(Buttons.DPadLeft) || inputHelper.GetLeftControlerStick() == Direction.Left)
            velocity.X = -walkingSpeed;
        else if (inputHelper.IsKeyDown(Keys.Right) || inputHelper.IsKeyDown(Keys.D) || inputHelper.IsControlerButtonDown(Buttons.DPadRight) || inputHelper.GetLeftControlerStick() == Direction.Right)
            velocity.X = walkingSpeed;
        else if (!walkingOnIce && isOnTheGround)
            velocity.X = 0.0f;
        if (velocity.X != 0.0f)
            Mirror = velocity.X < 0;
        if ((inputHelper.KeyPressed(Keys.Up) || inputHelper.KeyPressed(Keys.W) || inputHelper.ControlerButtonPressed(Buttons.DPadUp) || inputHelper.ControlerButtonPressed(Buttons.A)) && isOnTheGround)
            Jump();
        if (inputHelper.KeyPressed(Keys.Space) || inputHelper.ControlerButtonPressed(Buttons.X))
            Shoot();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        foreach (Projectile projectile in projectiles)
        {
            projectile.Update(gameTime);
            if (projectile.Hit)
                projectile.Active = false;
        }

        if (!finished && isAlive)
        {
            if (isOnTheGround)
                if (velocity.X == 0)
                    PlayAnimation("idle");
                else
                    PlayAnimation("run");
            else if (velocity.Y < 0)
                PlayAnimation("jump");

            TimerGameObject timer = GameWorld.Find("timer") as TimerGameObject;
            if (walkingOnHot)
                timer.Multiplier = 2;
            else if (walkingOnIce)
                timer.Multiplier = 0.5;
            else
                timer.Multiplier = 1;

            TileField tiles = GameWorld.Find("tiles") as TileField;
            if (BoundingBox.Top >= tiles.Rows * tiles.CellHeight)
                Die(true);
        }

        DoPhysics();
        GameEnvironment.ActiveCamera.moveCamera(Position + Center);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);

        foreach (Projectile projectile in projectiles)
            projectile.Draw(gameTime, spriteBatch);
    }

    public void Explode()
    {
        if (!isAlive || finished)
            return;
        isAlive = false;
        exploded = true;
        velocity = Vector2.Zero;
        position.Y += 15;
        PlayAnimation("explode");
    }

    public void Die(bool falling)
    {
        if (!isAlive || finished)
            return;
        isAlive = false;
        velocity.X = 0.0f;
        if (falling)
            GameEnvironment.AssetManager.PlaySound("Sounds/snd_player_fall");
        else
        {
            velocity.Y = -900;
            GameEnvironment.AssetManager.PlaySound("Sounds/snd_player_die");
        }
        PlayAnimation("die");
    }

    public bool IsAlive
    { get { return isAlive; } }

    public bool Finished
    { get { return finished; } }

    public List<Projectile> Projectiles
    { get { return projectiles; } }

    public void LevelFinished()
    {
        finished = true;
        velocity.X = 0.0f;
        PlayAnimation("celebrate");
        GameEnvironment.AssetManager.PlaySound("Sounds/snd_player_won");
    }

    public void Shoot()
    {
        Projectile projectile = new Projectile(position, Mirror);
        projectiles.Add(projectile);
        projectile.Parent = this;
    }
}

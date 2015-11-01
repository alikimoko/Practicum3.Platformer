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
    protected const int maxhp = 100;
    protected int hp;
    
    /// <summary>Create a new player.</summary>
    /// <param name="start">The players starting position.</param>
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

    /// <summary>Reset the player.</summary>
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
        hp = maxhp;
    }

    /// <summary>Handle user input.</summary>
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

    /// <summary>Update the player.</summary>
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

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

    /// <summary>Draw the player and its life bar.</summary>
    /// <param name="gameTime"></param>
    /// <param name="spriteBatch"></param>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);

        if (visible && hp != maxhp && hp != 0)
            DrawingHelper.DrawStatusBar(spriteBatch, GlobalPosition - new Vector2(0, 100) - GameEnvironment.ActiveCamera.Position,
                                        new Vector2(100, 10), maxhp, hp,
                                        new Color(255 - (int)(255 * (float)hp / maxhp), (int)(255 * (float)hp / maxhp), 0));
    }

    /// <summary>Make the player explode.</summary>
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

    /// <summary>Lower the players health and check for death.</summary>
    /// <param name="HP">The ammount of health points to decrease.</param>
    public void LowerHP(int HP)
    {
        if (!isAlive || finished)
            return;
        hp -= HP;
        if (hp <= 0)
        {
            hp = 0; // for drawing
            Die(false);
        }
    }

    /// <summary>Theplayer dies.</summary>
    /// <param name="falling">Did the player die by falling out of the level?</param>
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
    {
        get
        {
            Level level = parent as Level;
            return level.Projectiles;
        }
    }

    public void LevelFinished()
    {
        finished = true;
        velocity.X = 0.0f;
        PlayAnimation("celebrate");
        GameEnvironment.AssetManager.PlaySound("Sounds/snd_player_won");
    }

    public void Shoot()
    {
        Level level = parent as Level;
        if (level.activeProjectiles() < 5)
        {
            Projectile projectile = new Projectile(position, Mirror);
            Projectiles.Add(projectile);
        }
    }
}

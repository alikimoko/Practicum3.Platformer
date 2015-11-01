using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public enum Direction { Stay, Left, Right };

public class InputHelper
{
    protected MouseState currentMouseState, previousMouseState;
    protected KeyboardState currentKeyboardState, previousKeyboardState;
    protected GamePadState currentControlerState, previousControlerState;
    protected Vector2 scale;

    /// <summary>Create an input helper.</summary>
    public InputHelper()
    {
        scale = Vector2.One;
    }

    /// <summary>Update player inputs.</summary>
    public void Update()
    {
        previousMouseState = currentMouseState;
        previousKeyboardState = currentKeyboardState;
        previousControlerState = currentControlerState;
        currentMouseState = Mouse.GetState();
        currentKeyboardState = Keyboard.GetState();
        currentControlerState = GamePad.GetState(PlayerIndex.One);
    }

    /// <summary>The scale for the mouse.</summary>
    public Vector2 Scale
    {
        get { return scale; }
        set { scale = value; }
    }

    /// <summary>Get the mouse position.</summary>
    public Vector2 MousePosition
    { get { return new Vector2(currentMouseState.X, currentMouseState.Y) / scale; } }

    /// <summary>Has the left mouse button been pressed?</summary>
    public bool MouseLeftButtonPressed()
    { return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released; }

    /// <summary>Is the left mouse button down?</summary>
    public bool MouseLeftButtonDown()
    { return currentMouseState.LeftButton == ButtonState.Pressed; }

    /// <summary>Has the key been pressed?</summary>
    /// <param name="k">The key to check.</param>
    public bool KeyPressed(Keys k)
    { return currentKeyboardState.IsKeyDown(k) && previousKeyboardState.IsKeyUp(k); }

    /// <summary>Is the key down?</summary>
    /// <param name="k">The key to check.</param>
    public bool IsKeyDown(Keys k)
    { return currentKeyboardState.IsKeyDown(k); }

    /// <summary>Has any key being pressed?</summary>
    public bool AnyKeyPressed
    { get { return currentKeyboardState.GetPressedKeys().Length > 0 && previousKeyboardState.GetPressedKeys().Length == 0; } }

    /// <summary>Is the controler button pressed?</summary>
    /// <param name="b">The button to check.</param>
    public bool ControlerButtonPressed(Buttons b)
    { return currentControlerState.IsButtonDown(b) && previousControlerState.IsButtonUp(b); }

    /// <summary>Is the controler button bown?</summary>
    /// <param name="b">The button to check.</param>
    public bool IsControlerButtonDown(Buttons b)
    { return currentControlerState.IsButtonDown(b); }

    /// <summary>Get the direction of the left yoystick.</summary>
    public Direction GetLeftControlerStick()
    {
        float stickLeftX = currentControlerState.ThumbSticks.Left.X;
        if (stickLeftX < 0)
        { return Direction.Left; }
        else if (stickLeftX > 0)
        { return Direction.Right; }
        return Direction.Stay;            
    }
}
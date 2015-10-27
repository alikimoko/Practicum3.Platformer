class Button : SpriteGameObject
{
    protected bool pressed;

    /// <summary>Create a new button.</summary>
    /// <param name="imageAsset">The buton sprite.</param>
    /// <param name="layer">The layer the button is in.</param>
    /// <param name="id">The ID to reference the button.</param>
    public Button(string imageAsset, int layer = 0, string id = "")
        : base(imageAsset, layer, id)
    { pressed = false; }

    /// <summary>Check if the button is pressed.</summary>
    public override void HandleInput(InputHelper inputHelper)
    {
        pressed = inputHelper.MouseLeftButtonPressed() &&
            BoundingBox.Contains((int)inputHelper.MousePosition.X, (int)inputHelper.MousePosition.Y);
    }

    /// <summary>Reset the button.</summary>
    public override void Reset()
    {
        base.Reset();
        pressed = false;
    }

    /// <summary>Is the button pressed?</summary>
    public bool Pressed
    { get { return pressed; } }
}

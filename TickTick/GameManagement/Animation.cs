using System;
using Microsoft.Xna.Framework;

public class Animation : SpriteSheet
{
    protected float frameTime;
    protected bool isLooping;
    protected float time;

    /// <summary>Create a new animation.</summary>
    /// <param name="assetname">The name of the animation file.</param>
    /// <param name="isLooping">Is the animation looping?</param>
    /// <param name="FPS">The amount of frames that should be shown per second.</param>
    public Animation(string assetname, bool isLooping, int FPS = 10) : this(assetname, isLooping, 1f / FPS) { }

    /// <summary>Create a new animation.</summary>
    /// <param name="assetname">The name of the animation file.</param>
    /// <param name="isLooping">Is the animation looping?</param>
    /// <param name="frameTime">The time in seconds a frame lasts.</param>
    public Animation(string assetname, bool isLooping, float frameTime = 0.1f) : base(assetname)
    {
        this.frameTime = frameTime;
        this.isLooping = isLooping;
    }

    /// <summary>Start the animation from the first frame.</summary>
    public void Play()
    {
        sheetIndex = 0;
        time = 0.0f;
    }

    /// <summary>Update the animation.</summary>
    public void Update(GameTime gameTime)
    {
        time += (float)gameTime.ElapsedGameTime.TotalSeconds;
        while (time > frameTime)
        {
            time -= frameTime;
            if (isLooping)
                sheetIndex = (sheetIndex + 1) % NumberSheetElements;
            else
                sheetIndex = Math.Min(sheetIndex + 1, NumberSheetElements - 1);
        }
    }

    /// <summary>Get the amount of frames the animation displays per second.</summary>
    public int FPS
    { get { return (int)(1 / frameTime); } }

    /// <summary>Get the seconds per frame.</summary>
    public float FrameTime
    { get { return frameTime; } }

    /// <summary>Is this animation playing in a loop?</summary>
    public bool IsLooping
    { get { return isLooping; } }

    /// <summary>The amount of frames in this animation.</summary>
    public int CountFrames
    { get { return NumberSheetElements; } }

    /// <summary>Has the animation ended?</summary>
    public bool AnimationEnded
    { get { return !isLooping && sheetIndex >= NumberSheetElements - 1; } }
}


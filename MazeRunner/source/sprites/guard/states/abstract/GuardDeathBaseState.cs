﻿using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public abstract class GuardDeathBaseState : GuardBaseState
{
    protected GuardDeathBaseState(ISpriteState previousState) : base(previousState)
    {
    }

    public override Texture2D Texture => Textures.Sprites.Guard.Dead;

    public override int FramesCount
    {
        get
        {
            return 4;
        }
    }
}
﻿using MazeRunner.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Gui.Buttons.States;

public class MenuButtonResetState : ButtonPushBaseState
{
    public override Texture2D Texture => Textures.Gui.Buttons.Menu.Click;

    public override int FramesCount => 5;

    public MenuButtonResetState(Button button) : base(button)
    {
        var framePosX = (FramesCount - 1) * FrameWidth;

        CurrentAnimationFramePoint = new Point(framePosX, 0);
    }

    public override IButtonState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            var animationPoint = CurrentAnimationFramePoint;

            if (animationPoint.X is 0)
            {
                return new MenuButtonIdleState(Button);
            }

            var framePosX = animationPoint.X - FrameWidth;

            CurrentAnimationFramePoint = new Point(framePosX, 0);
            ElapsedGameTimeMs -= UpdateTimeDelayMs;
        }

        return this;
    }
}
﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using static MazeRunner.Settings;

namespace MazeRunner.Managers;

public static class KeyboardManager
{
    public static IEnumerable<Vector2> ProcessHeroMovement()
    {
        if (Keyboard.GetState().IsKeyDown(MoveUp))
        {
            yield return -Vector2.UnitY;
        }

        if (Keyboard.GetState().IsKeyDown(MoveDown))
        {
            yield return Vector2.UnitY;
        }

        if (Keyboard.GetState().IsKeyDown(MoveLeft))
        {
            yield return -Vector2.UnitX;
        }

        if (Keyboard.GetState().IsKeyDown(MoveRight))
        {
            yield return Vector2.UnitX;
        }
    }
}
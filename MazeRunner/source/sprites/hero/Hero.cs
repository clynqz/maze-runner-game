﻿using MazeRunner.Extensions;
using MazeRunner.Helpers;
using MazeRunner.Managers;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Sprites.States;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using System;

namespace MazeRunner.Sprites;

public class Hero : Sprite
{
    private const double MovePollingTimeMs = 15;

    private const int HitBoxOffsetX = 3;
    private const int HitBoxOffsetY = 3;

    private const int HitBoxWidth = 10;
    private const int HitBoxHeight = 12;

    private double _movementPollingTimeMs;

    public override Vector2 Speed
    {
        get
        {
            return new Vector2(1, 1);
        }
    }

    protected override ISpriteState State { get; set; }

    public Hero()
    {
        State = new HeroIdleState();
    }

    public override Rectangle GetHitBox(Vector2 position)
    {
        return new Rectangle(
                (int)position.X + HitBoxOffsetX,
                (int)position.Y + HitBoxOffsetY,
                HitBoxWidth,
                HitBoxHeight);
    }

    public override void Update(MazeRunnerGame game, GameTime gameTime)
    {
        void ProcessState(Vector2 movement)
        {
            if (movement == Vector2.Zero)
            {
                if (State is not HeroIdleState)
                {
                    State = new HeroIdleState();
                }
            }
            else
            {
                if (State is not HeroRunState)
                {
                    State = new HeroRunState();
                }
            }
        }

        base.Update(game, gameTime);

        if (!KeyboardManager.IsPollingTimePassed(MovePollingTimeMs, ref _movementPollingTimeMs, gameTime))
        {
            return;
        }

        var heroInfo = game.HeroInfo;
        var position = heroInfo.Position;

        var mazeInfo = game.MazeInfo;

        var movement = ProcessMovement(position, mazeInfo.Maze);

        FrameEffect = SpriteBaseState.ProcessFrameEffect(movement, FrameEffect);
        ProcessState(movement);

        position += movement;

        ProcessItemsColliding(position, mazeInfo);

        heroInfo.Position = position;
    }

    #region Collidings
    private void ProcessItemsColliding(Vector2 position, MazeInfo mazeInfo)
    {
        void ProcessKeyColliding(Vector2 position, Cell cell, Key key)
        {
            if (CollisionManager.CollidesWithKey(this, position, cell, key))
            {
                mazeInfo.Maze.RemoveItem(cell);
                mazeInfo.IsKeyCollected = true;
            }
        }

        var maze = mazeInfo.Maze;

        if (CollisionManager.CollidesWithItems(this, position, maze, out var itemInfo))
        {
            var (cell, item) = itemInfo;

            if (item is Key key)
            {
                ProcessKeyColliding(position, cell, key);
            }
        }
    }
    #endregion

    #region MovementCalculations
    private Vector2 ProcessMovement(Vector2 position, Maze maze)
    {
        Vector2 NormalizeDiagonalSpeed(Vector2 movement)
        {
            var speed = Speed;

            if (movement.Abs() == speed)
            {
                var normalizedSpeed = new Vector2(movement.X / Math.Abs(movement.X) * .7f, movement.Y / Math.Abs(movement.Y) * .7f);

                return normalizedSpeed;
            }

            return movement;
        }

        Vector2 GetTotalMovement(Maze maze, Vector2 movement, Vector2 position)
        {
            var totalMovement = Vector2.Zero;

            var movementX = new Vector2(movement.X, 0);
            var movementY = new Vector2(0, movement.Y);

            if (!CollisionManager.CollidesWithWalls(this, position, movementX, maze)
             && !CollisionManager.CollidesWithExit(this, position, movementX, maze))
            {
                totalMovement += movementX;
            }

            if (!CollisionManager.CollidesWithWalls(this, position, movementY, maze)
             && !CollisionManager.CollidesWithExit(this, position, movementY, maze))
            {
                totalMovement += movementY;
            }

            if (ProcessDiagonalMovement(maze, totalMovement, position, movementX, movementY, out totalMovement))
            {
                return totalMovement;
            }

            return totalMovement;
            //return NormalizeDiagonalSpeed(totalMovement);
        }

        bool ProcessDiagonalMovement(Maze maze, Vector2 movement, Vector2 position, Vector2 movementX, Vector2 movementY, out Vector2 totalMovement) //
        {
            if (CollisionManager.CollidesWithWalls(this, position, movement, maze))
            {
                if (RandomHelper.RandomBoolean())
                {
                    totalMovement = movementX;
                }
                else
                {
                    totalMovement = movementY;
                }

                return true;
            }

            totalMovement = movement;

            return false;
        }

        var movement = KeyboardManager.ProcessHeroMovement(this);
        var totalMovement = GetTotalMovement(maze, movement, position);

        return totalMovement;
    }
    #endregion
}
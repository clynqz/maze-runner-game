﻿using MazeRunner.Extensions;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using System.Collections.Immutable;

namespace MazeRunner.Managers;

public static class CollisionManager
{
    public static bool CollidesWithWalls(SpriteInfo spriteInfo, Vector2 movement, Maze maze)
    {
        var mazeSkeleton = maze.Skeleton;

        for (int y = 0; y < mazeSkeleton.GetLength(0); y++)
        {
            for (int x = 0; x < mazeSkeleton.GetLength(1); x++)
            {
                var tile = mazeSkeleton[y, x];

                if (tile.TileType is not TileType.Wall)
                {
                    continue;
                }

                if (CollidesWithMazeTile(spriteInfo, movement, tile, new Cell(x, y)))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static bool CollidesWithExit(SpriteInfo spriteInfo, Vector2 movement, Maze maze)
    {
        var (exitCell, exit) = maze.ExitInfo;

        return CollidesWithMazeTile(spriteInfo, movement, exit, exitCell)
           && !exit.IsOpened;
    }

    public static bool CollidesWithItems(SpriteInfo spriteInfo, Maze maze, out (Cell Cell, MazeItem Item) itemInfo)
    {
        if (CollidesWith(maze.ItemsInfo, spriteInfo, out var tileInfo))
        {
            itemInfo = (tileInfo.Cell, (MazeItem)tileInfo.Tile);
            return true;
        }

        itemInfo = (new Cell(), null);
        return false;
    }

    public static bool CollidesWithTraps(SpriteInfo spriteInfo, Maze maze, out (Cell Cell, MazeTrap Trap) trapInfo)
    {
        if (CollidesWith(maze.TrapsInfo, spriteInfo, out var tileInfo) && ((MazeTrap)tileInfo.Tile).IsActivated)
        {
            trapInfo = (tileInfo.Cell, (MazeTrap)tileInfo.Tile);
            return true;
        }

        trapInfo = (new Cell(), null);
        return false;
    }

    private static bool CollidesWith(ImmutableDictionary<Cell, MazeTile> sourceInfo, SpriteInfo spriteInfo, out (Cell Cell, MazeTile Tile) tileInfo)
    {
        foreach (var (cell, tile) in sourceInfo)
        {
            if (CollidesWithMazeTile(spriteInfo, tile, cell))
            {
                tileInfo = (cell, tile);

                return true;
            }
        }

        tileInfo = (new Cell(), null);
        return false;
    }

    private static bool CollidesWithMazeTile(SpriteInfo spriteInfo, MazeTile mazeTile, Cell tileCell)
    {
        return CollidesWithMazeTile(spriteInfo, Vector2.Zero, mazeTile, tileCell);
    }

    private static bool CollidesWithMazeTile(SpriteInfo spriteInfo, Vector2 movement, MazeTile mazeTile, Cell tileCell)
    {
        var tilePosition = Maze.GetIndependentCellPosition(mazeTile, tileCell);
        var tileHitBox = mazeTile.GetHitBox(tilePosition);

        return GetExtendedHitBox(spriteInfo, movement).Intersects(tileHitBox);
    }

    private static FloatRectangle GetExtendedHitBox(SpriteInfo spriteInfo, Vector2 movement)
    {
        var sprite = spriteInfo.Sprite;
        var position = spriteInfo.Position;

        var hitBox = sprite.GetHitBox(position);

        var x = hitBox.X;
        var y = hitBox.Y;
        var width = hitBox.Width;
        var height = hitBox.Height;

        if (movement == Vector2.Zero)
        {
            goto _return;
        }

        if (movement.X > 0)
        {
            width += movement.X;
        }
        else if (movement.X < 0)
        {
            x += movement.X;
        }

        if (movement.Y > 0)
        {
            height += movement.Y;
        }
        else if (movement.Y < 0)
        {
            y += movement.Y;
        }

    _return:
        return new FloatRectangle(x, y, width, height);
    }
}
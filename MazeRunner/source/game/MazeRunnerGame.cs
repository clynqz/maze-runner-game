﻿#region Usings
using static MazeRunner.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
#endregion

namespace MazeRunner;

public class MazeRunnerGame : Game
{
    private GraphicsDeviceManager _graphics;

    private Maze _maze;
    private readonly Drawer _drawer = Drawer.GetInstance();

    public MazeRunnerGame()
    {
        _graphics = new(this)
        {
            PreferredBackBufferWidth = WindowWidth,
            PreferredBackBufferHeight = WindowHeight,
        };

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        _maze = MazeGenerator.GenerateMaze(MazeWidth, MazeHeight);
        _maze.LoadToFile(new System.IO.FileInfo("maze.txt"));

        _drawer.Initialize(this);
    }

    protected override void LoadContent()
    {
        _drawer.LoadContent(this);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        //GraphicsDevice.Clear(Color.White);

        _drawer.BeginDraw();

        _drawer.DrawMaze(_maze);

        _drawer.EndDraw();

        base.Draw(gameTime);
    }
}
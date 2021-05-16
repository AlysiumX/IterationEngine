using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace IterationEngine.MonoProject.Editor
{
    public class MenuItem
    {
        public TileSheetTile Tile { get; }
        public Action Process { get; }

        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }


        public MenuItem( TileSheetTile tile, int x, int y, int width, int height, Action process )
        {
            Tile = tile;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Process = process;
        }

        public bool IsPointOnItem( Point point )
        {
            return
                point.X >= X && point.X < X + Width &&
                point.Y >= Y && point.Y < Y + Height;
        }
    }
}

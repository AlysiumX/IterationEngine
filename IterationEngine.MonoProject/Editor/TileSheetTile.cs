using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace IterationEngine.MonoProject.Editor
{
    public class TileSheetTile
    {
        private int TileSize = Globals.GameSettings.TileSize;

        public Texture2D Image { get; set; }
        public int X { get { return TileSheetX * TileSize; } }
        public int Y { get { return TileSheetY * TileSize; } }
        public int TileSheetX { get; set; }
        public int TileSheetY { get; set; }
        public int Width { get { return Image.Width; } }
        public int Height { get { return Image.Height; } }

        public TileSheetTile( Texture2D image, int tileSheetX, int tileSheetY )
        {
            Image = image;
            TileSheetX = tileSheetX;
            TileSheetY = tileSheetY;
        }

        public bool IsPointOnTile( Point point )
        {
            return
                point.X >= X && point.X < X + Width &&
                point.Y >= Y && point.Y < Y + Height;
        }
    }
}

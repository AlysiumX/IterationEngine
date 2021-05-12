using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace IterationEngine.MonoProject
{
    public class MapTile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public TileSheetTile TileSheetTile { get; set; }

        public MapTile( int x, int y, TileSheetTile tileSheetTile )
        {
            X = x;
            Y = y;
            TileSheetTile = tileSheetTile;
        }
    }
}

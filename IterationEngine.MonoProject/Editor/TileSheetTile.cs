using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace IterationEngine.MonoProject
{
    public class TileSheetTile
    {
        public Texture2D Image { get; set; }
        public int TileSheetX { get; set; }
        public int TileSheetY { get; set; }

        public TileSheetTile( Texture2D image, int tileSheetX, int tileSheetY )
        {
            Image = image;
            TileSheetX = tileSheetX;
            TileSheetY = tileSheetY;
        }
    }
}

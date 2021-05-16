using System;
using System.Collections.Generic;
using System.Text;

namespace IterationEngine.MonoProject
{
    public class MenuItem
    {
        public TileSheetTile Tile { get; }
        public Action Process { get; }

        public MenuItem( TileSheetTile tile, Action process )
        {
            Tile = tile;
            Process = process;
        }
    }
}

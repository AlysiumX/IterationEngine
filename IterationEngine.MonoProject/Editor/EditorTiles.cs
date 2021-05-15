using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace IterationEngine.MonoProject
{
    public static class EditorTiles
    {
        public static TileSheetTile GridTile { get; private set; }
        public static TileSheetTile MenuBar { get; private set; }

        private static GraphicsDevice _graphicsDevice { get { return Globals.GraphicsDevice; } }
        private static SpriteBatch _spriteBatch { get { return Globals.SpriteBatch; } }

        private static Texture2D _editorTileSheet = Texture2D.FromFile( _graphicsDevice, $"{AppDomain.CurrentDomain.BaseDirectory}/Content/EditorTiles.png" );

        public static void Initialize()
        {
            GridTile = new TileSheetTile( _editorTileSheet, 0, 0 );
            MenuBar = new TileSheetTile( _editorTileSheet, 1, 0 );
        }
    }
}

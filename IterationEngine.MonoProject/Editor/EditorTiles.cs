using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace IterationEngine.MonoProject
{
    public static class EditorTiles
    {
        public static TileSheetTile GridTile { get; private set; }
        public static TileSheetTile MenuBackground { get; private set; }
        public static TileSheetTile New { get; private set; }
        public static TileSheetTile Open { get; private set; }
        public static TileSheetTile Save { get; private set; }

        private static GraphicsDevice _graphicsDevice { get { return Globals.GraphicsDevice; } }
        private static SpriteBatch _spriteBatch { get { return Globals.SpriteBatch; } }

        private static Texture2D _editorTileSheet = Texture2D.FromFile( _graphicsDevice, $"{AppDomain.CurrentDomain.BaseDirectory}/Content/EditorTiles.png" );

        public static void Initialize()
        {
            GridTile = new TileSheetTile( _editorTileSheet, 0, 0 );
            MenuBackground = new TileSheetTile( _editorTileSheet, 1, 0 );
            New = new TileSheetTile( _editorTileSheet, 2, 0 );
            Open = new TileSheetTile( _editorTileSheet, 3, 0 );
            Save = new TileSheetTile( _editorTileSheet, 4, 0 );
        }
    }
}

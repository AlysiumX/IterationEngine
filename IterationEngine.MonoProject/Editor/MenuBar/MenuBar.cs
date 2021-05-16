using IterationEngine.MonoProject.Editor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IterationEngine.MonoProject.Editor
{
    public class MenuBar
    {
        private SpriteBatch _spriteBatch { get { return Globals.SpriteBatch; } }
        private int TileSize = 16; //Tile size of menu icons does not change from the tile size of the game.

        private List<MenuItem> _menuItems;

        private TileSheetTile MenuBackground { get { return EditorTiles.MenuBackground; } }

        private int _spaceBetweenItems = 8;
        private int _iconWidthAndHeight = 32;

        public MenuBar()
        {
            _menuItems = new List<MenuItem>
            {
                new MenuItem(EditorTiles.New, EditorOperations.CreateNewMap ),
                new MenuItem(EditorTiles.Open, EditorOperations.OpenMap ),
                new MenuItem(EditorTiles.Save, EditorOperations.SaveMap ),
                new MenuItem(EditorTiles.Play, EditorOperations.PlayMap )
            };
        }

        public void Update( GameTime gameTime )
        {
            if( Mouse.GetState().LeftButton == ButtonState.Pressed )
            {
                var clickedMenuItem = GetClickedMenuItemFromMenuBar( Mouse.GetState().Position );
                if( clickedMenuItem != null )
                {
                    clickedMenuItem.Process();
                }
            }
        }

        private MenuItem GetClickedMenuItemFromMenuBar( Point mousePosition )
        {
            return _menuItems.Where( x => x.Tile.IsPointOnTile( mousePosition ) ).FirstOrDefault();
        }

        public void Draw( GameTime gameTime )
        {
            _spriteBatch.Begin( samplerState: SamplerState.PointClamp );
            _spriteBatch.Draw( MenuBackground.Image, new Rectangle( 0, 0, Globals.GameSettings.GameWidth, 32 ), new Rectangle( MenuBackground.TileSheetX * TileSize, MenuBackground.TileSheetY * TileSize, TileSize, TileSize ), Color.White );
            for( var i = 0; i < _menuItems.Count; i++ )
            {
                var iconLocationX = ( ( _spaceBetweenItems * ( i + 1 ) ) + ( _iconWidthAndHeight * i ) );
                _spriteBatch.Draw( _menuItems[i].Tile.Image, new Rectangle( iconLocationX, 0, 32, 32 ), new Rectangle( _menuItems[i].Tile.TileSheetX * TileSize, _menuItems[i].Tile.TileSheetY * TileSize, TileSize, TileSize ), Color.White );
            }

            _spriteBatch.End();
        }
    }
}

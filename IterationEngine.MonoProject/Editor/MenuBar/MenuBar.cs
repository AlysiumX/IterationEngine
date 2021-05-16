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
        private const int TileSize = 16; //Tile size of menu icons does not change from the tile size of the game.
        private const int MenuBarHeight = 32;

        private List<MenuItem> _menuItems;

        private TileSheetTile MenuBackground { get { return EditorTiles.MenuBackground; } }


        public MenuBar()
        {
            _menuItems = new List<MenuItem>
            {
                new MenuItem(EditorTiles.New, 8, 0, 32, 32, EditorOperations.CreateNewMap ),
                new MenuItem(EditorTiles.Open, 48, 0, 32, 32, EditorOperations.OpenMap ),
                new MenuItem(EditorTiles.Save, 88, 0, 32, 32, EditorOperations.SaveMap ),
                new MenuItem(EditorTiles.Play, 128, 0, 32, 32, EditorOperations.PlayMap )
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
            return _menuItems.Where( x => x.IsPointOnItem( mousePosition ) ).FirstOrDefault();
        }

        public void Draw( GameTime gameTime )
        {
            _spriteBatch.Begin( samplerState: SamplerState.PointClamp );
            _spriteBatch.Draw( MenuBackground.Image, new Rectangle( 0, 0, Globals.GameSettings.GameWidth, MenuBarHeight ), new Rectangle( MenuBackground.TileSheetX * TileSize, MenuBackground.TileSheetY * TileSize, TileSize, TileSize ), Color.White );
            foreach( var menuItem in _menuItems )
            {
                _spriteBatch.Draw( menuItem.Tile.Image, new Rectangle( menuItem.X, menuItem.Y, menuItem.Width, menuItem.Height ), new Rectangle( menuItem.Tile.TileSheetX * TileSize, menuItem.Tile.TileSheetY * TileSize, TileSize, TileSize ), Color.White );
            }

            _spriteBatch.End();
        }
    }
}

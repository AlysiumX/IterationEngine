using IterationEngine.MonoProject.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IterationEngine.MonoProject.Editor
{
    public class MapEditor : GameState
    {
        private GraphicsDevice _graphicDevice { get { return Globals.GraphicsDevice; } }
        private SpriteBatch _spriteBatch { get { return Globals.SpriteBatch; } }
        private TileSheetTile EditorTile { get { return EditorTiles.GridTile; } }

        public int TileSize = Globals.GameSettings.TileSize;

        private List<MapTile> ChangedTiles { get; set; }
        private int targetMapWidth = 10000;
        private int targetMapHeight = 10000;

        private int currentMapTileWidth = 0;
        private int currentMapTileHeight = 0;

        private Camera _camera;
        private RenderTarget2D _mapTexture;
        private MenuBar _menuBar;

        private Texture2D ExampleTileSheet { get; set; }
        public TileSheetTile CurrentlySelectedTile { get; set; }

        private TileSheetBrowser _tileSheetBrowser { get; set; }
        private Rectangle TileSheetButton = new Rectangle( 8, Globals.GameSettings.GameHeight - 64 - 8, 64, 64 );

        public MapEditor() { }

        public void Initialize()
        {
            EditorTiles.Initialize();

            ChangedTiles = new List<MapTile>();

            ExampleTileSheet = Texture2D.FromFile( _graphicDevice, $"{AppDomain.CurrentDomain.BaseDirectory}/Content/ExampleTileSheet.png" );
            CurrentlySelectedTile = new TileSheetTile( ExampleTileSheet, 0, 0 );

            SetMapTileSize();

            var cameraStartingPosition = new Vector2( ( currentMapTileWidth * TileSize ) / 2, ( currentMapTileHeight * TileSize ) / 2 );
            _camera = new Camera( _graphicDevice, _graphicDevice.Viewport, cameraStartingPosition, currentMapTileWidth * TileSize, currentMapTileHeight * TileSize, 1 );
            _camera.SetZoomLimitsBasedOnTileSize( TileSize );

            RebuildMapToMapTexture();

            _menuBar = new MenuBar();

            _tileSheetBrowser = new TileSheetBrowser();
            _tileSheetBrowser.Initialize();
            _tileSheetBrowser.SetParent( this );
        }

        private void SetMapTileSize()
        {
            currentMapTileWidth = targetMapWidth / TileSize;
            currentMapTileHeight = targetMapHeight / TileSize;
        }

        private void RebuildMapToMapTexture()
        {
            _mapTexture = new RenderTarget2D( _graphicDevice, currentMapTileWidth * TileSize, currentMapTileHeight * TileSize );
            _graphicDevice.SetRenderTarget( _mapTexture );
            _spriteBatch.Begin( samplerState: SamplerState.PointClamp );
            for( var y = 0; y < currentMapTileHeight; y++ )
            {
                for( var x = 0; x < currentMapTileWidth; x++ )
                {
                    _spriteBatch.Draw( EditorTile.Image, new Rectangle( x * TileSize, y * TileSize, TileSize, TileSize ), new Rectangle( EditorTile.TileSheetX * TileSize, EditorTile.TileSheetY * TileSize, TileSize, TileSize ), Color.White );
                }
            }

            _spriteBatch.End();
            _graphicDevice.SetRenderTarget( null );
        }

        public void Update( GameTime gameTime )
        {
            if( _tileSheetBrowser.Shown )
            {
                _tileSheetBrowser.Update( gameTime );
                return;
            }

            _camera.Update( gameTime );

            if( Mouse.GetState().LeftButton == ButtonState.Pressed && !Input.MousePreviouslyPress )
            {
                if( CheckIfSelectedTileClicked( Mouse.GetState().Position ) )
                {
                    Input.MousePreviouslyPress = true;
                    if( !_tileSheetBrowser.Shown )
                        _tileSheetBrowser.Show( ExampleTileSheet );


                    return;
                }

                ChangeTileToTile( _camera.GetMousePosition(), CurrentlySelectedTile );
            }

            _menuBar.Update( gameTime );

            if( Mouse.GetState().LeftButton == ButtonState.Released )
            {
                Input.MousePreviouslyPress = false;
            }

            if( Mouse.GetState().RightButton == ButtonState.Pressed )
            {
                RemoveTile( _camera.GetMousePosition() );
            }
        }

        private bool CheckIfSelectedTileClicked( Point position )
        {
            return position.X > TileSheetButton.X &&
                   position.X < TileSheetButton.X + TileSheetButton.Width &&
                   position.Y > TileSheetButton.Y &&
                   position.Y < TileSheetButton.Y + TileSheetButton.Height;
        }

        private void ChangeTileToTile( Vector2 mouseActualPosition, TileSheetTile tileSheetTile )
        {
            var tileX = (int)Math.Floor( mouseActualPosition.X / TileSize );
            var tileY = (int)Math.Floor( mouseActualPosition.Y / TileSize );
            var mapTile = ChangedTiles.Where( tile => tile.X == tileX && tile.Y == tileY ).FirstOrDefault();

            if( mapTile != null )
            {
                if( mapTile.TileSheetTile == EditorTile )
                {
                    ChangedTiles.Remove( mapTile );
                }

                mapTile.TileSheetTile = tileSheetTile;
            }
            else
            {
                mapTile = new MapTile( tileX, tileY, tileSheetTile );
                ChangedTiles.Add( mapTile );
            }
        }

        private void RemoveTile( Vector2 mouseActualPosition )
        {
            var tileX = (int)Math.Floor( mouseActualPosition.X / TileSize );
            var tileY = (int)Math.Floor( mouseActualPosition.Y / TileSize );
            var mapTile = ChangedTiles.Where( tile => tile.X == tileX && tile.Y == tileY ).FirstOrDefault();
            if( mapTile != null )
            {
                ChangedTiles.Remove( mapTile );
            }
        }

        public void Draw( GameTime gameTime )
        {
            if( _tileSheetBrowser.Shown )
            {
                _tileSheetBrowser.Draw( gameTime );
                return;
            }

            _graphicDevice.Clear( Color.CornflowerBlue );
            _graphicDevice.SetRenderTarget( null );

            RenderLevel( gameTime );
            RenderUIElements( gameTime );
        }

        private void RenderLevel( GameTime gameTime )
        {
            _spriteBatch.Begin( SpriteSortMode.BackToFront, null, SamplerState.PointClamp, null, null, null, _camera.GetTransformation() );
            _spriteBatch.Draw( _mapTexture, Vector2.Zero, Color.White );
            foreach( var tile in ChangedTiles )
            {
                _spriteBatch.Draw( tile.TileSheetTile.Image, new Rectangle( tile.X * TileSize, tile.Y * TileSize, TileSize, TileSize ), new Rectangle( tile.TileSheetTile.TileSheetX * TileSize, tile.TileSheetTile.TileSheetY * TileSize, TileSize, TileSize ), Color.White );
            }
            _spriteBatch.End();

        }

        private void RenderUIElements( GameTime gameTime )
        {
            _spriteBatch.Begin( samplerState: SamplerState.PointClamp );
            _spriteBatch.Draw( CurrentlySelectedTile.Image, TileSheetButton, new Rectangle( CurrentlySelectedTile.TileSheetX * TileSize, CurrentlySelectedTile.TileSheetY * TileSize, TileSize, TileSize ), Color.White );
            _spriteBatch.End();

            _menuBar.Draw( gameTime );
        }
    }
}

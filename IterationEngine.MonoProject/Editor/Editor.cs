using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IterationEngine.MonoProject
{
    public class MapEditor : GameState
    {
        private List<MapTile> ChangedTiles { get; set; }
        private int targetMapWidth = 10000;
        private int targetMapHeight = 10000;

        private int currentMapTileWidth = 0;
        private int currentMapTileHeight = 0;

        private Camera _camera;

        private RenderTarget2D _mapTexture;

        //TODO : Load from Globals.TileSize
        public int TileSize = TileSizes.Small;

        public int TargetWidth { get; set; } = 640;
        public int TargetHeight { get; set; } = 360;

        private Texture2D EditorTileSheet { get; set; }
        private TileSheetTile EditorTile { get; set; }

        private Texture2D ExampleTileSheet { get; set; }
        private TileSheetTile CurrentlySelectedTile { get; set; }

        private GraphicsDevice _graphicDevice { get; set; }
        private SpriteBatch _spriteBatch { get; set; }


        public MapEditor() { }
        public void Initialize( GraphicsDevice graphicsDevice, SpriteBatch spriteBatch )
        {
            _graphicDevice = graphicsDevice;
            _spriteBatch = spriteBatch;

            ChangedTiles = new List<MapTile>();

            EditorTileSheet = Texture2D.FromFile( _graphicDevice, $"{AppDomain.CurrentDomain.BaseDirectory}/Content/EditorTiles.png" );
            ExampleTileSheet = Texture2D.FromFile( _graphicDevice, $"{AppDomain.CurrentDomain.BaseDirectory}/Content/ExampleTileSheet.png" );

            EditorTile = new TileSheetTile( EditorTileSheet, 0, 0 );
            CurrentlySelectedTile = new TileSheetTile( ExampleTileSheet, 0, 0 );

            SetMapTileSize();

            var cameraStartingPosition = new Vector2( ( currentMapTileWidth * TileSize ) / 2, ( currentMapTileHeight * TileSize ) / 2 );
            _camera = new Camera( _graphicDevice, _graphicDevice.Viewport, cameraStartingPosition, currentMapTileWidth * TileSize, currentMapTileHeight * TileSize, 1 );
            _camera.SetZoomLimitsBasedOnTileSize( TileSize );

            _mapTexture = new RenderTarget2D( _graphicDevice, currentMapTileWidth * TileSize, currentMapTileHeight * TileSize );

            RebuildMapToMapTexture();
        }

        private void SetMapTileSize()
        {
            currentMapTileWidth = targetMapWidth / TileSize;
            currentMapTileHeight = targetMapHeight / TileSize;
        }

        private void RebuildMapToMapTexture()
        {
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
            _camera.Update( gameTime );

            if( Mouse.GetState().LeftButton == ButtonState.Pressed )
            {
                ChangeTileToTile( _camera.GetMousePosition(), CurrentlySelectedTile );
            }

            if( Mouse.GetState().RightButton == ButtonState.Pressed )
            {
                ChangeTileToTile( _camera.GetMousePosition(), EditorTile );
            }
        }

        private void ChangeTileToTile( Vector2 mouseActualPosition, TileSheetTile tileSheetTile )
        {
            var tileX = (int)Math.Floor( mouseActualPosition.X / TileSize );
            var tileY = (int)Math.Floor( mouseActualPosition.Y / TileSize );
            var mapTile = ChangedTiles.Where( tile => tile.X == tileX && tile.Y == tileY ).FirstOrDefault();

            if( mapTile != null )
            {
                mapTile.TileSheetTile = tileSheetTile;
            }
            else
            {
                mapTile = new MapTile( tileX, tileY, tileSheetTile );
                ChangedTiles.Add( mapTile );
            }
        }

        public void Draw( GameTime gameTime )
        {
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
            //Currently Selected Tile
            _spriteBatch.Draw( CurrentlySelectedTile.Image, new Rectangle( 0, Globals.GameSettings.GameHeight - 64, 64, 64 ), new Rectangle( CurrentlySelectedTile.TileSheetX * TileSize, CurrentlySelectedTile.TileSheetY * TileSize, TileSize, TileSize ), Color.White );
            _spriteBatch.End();
        }
    }
}

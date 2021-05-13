using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace IterationEngine.MonoProject
{
    public class TileSheetBrowser : GameState
    {
        public bool Shown { get; set; }

        private GraphicsDevice _graphicDevice { get; set; }
        private SpriteBatch _spriteBatch { get; set; }

        private Texture2D CurrentlyLoadedTileSet { get; set; }
        private int TotalTileX { get; set; }
        private int TotalTileY { get; set; }

        private int TileSize { get; set; }

        private RenderTarget2D TileSetTarget { get; set; }

        private int ScaledWidth { get; set; }
        private int ScaledHeight { get; set; }
        private float Scale { get; set; }

        private int previousScroll;
        private int imageScrollValue = 0;
        private int scrollIncrement = 35;

        //TODO : Desperately need an input class!
        private bool mousePreviouslyPressed = true;

        private MapEditor _parent { get; set; }

        public void SetParent( MapEditor parent )
        {
            _parent = parent;
        }

        public void Initialize( GraphicsDevice graphicDevice, SpriteBatch spriteBatch )
        {
            _graphicDevice = graphicDevice;
            _spriteBatch = spriteBatch;

            TileSize = Globals.GameSettings.TileSize;
        }

        public void Show( Texture2D texture )
        {
            mousePreviouslyPressed = true;
            Shown = true;
            CurrentlyLoadedTileSet = texture;
            TotalTileX = texture.Width / TileSize;
            TotalTileY = texture.Height / TileSize;

            BuildTileSetTarget();

            Scale = (float)Globals.GameSettings.GameWidth / (float)CurrentlyLoadedTileSet.Width;

            ScaledWidth = Globals.GameSettings.GameWidth;
            ScaledHeight = CurrentlyLoadedTileSet.Height * (int)Math.Floor( Scale );
        }

        private void BuildTileSetTarget()
        {
            TileSetTarget = new RenderTarget2D( _graphicDevice, CurrentlyLoadedTileSet.Width, CurrentlyLoadedTileSet.Height );
            _graphicDevice.SetRenderTarget( TileSetTarget );
            _spriteBatch.Begin( samplerState: SamplerState.PointClamp );
            for( var y = 0; y<TotalTileY; y++ )
            {
                for( var x = 0; x<TotalTileX; x++ )
                {
                    _spriteBatch.Draw( CurrentlyLoadedTileSet, new Rectangle( x * TileSize, y * TileSize, TileSize, TileSize ), new Rectangle( x * TileSize, y * TileSize, TileSize, TileSize ), Color.White );
                }
            }
            _spriteBatch.End();
            _graphicDevice.SetRenderTarget( null );
        }

        public void Hide()
        {
            Shown = false;
        }

        public void Update( GameTime gameTime )
        {
            if( Mouse.GetState().ScrollWheelValue > previousScroll )
                ScrollImageUpIfPossible();
            else if( Mouse.GetState().ScrollWheelValue < previousScroll )
                ScrollImageDownIfPossible();

            previousScroll = Mouse.GetState().ScrollWheelValue;

            if( Mouse.GetState().LeftButton == ButtonState.Pressed && !mousePreviouslyPressed )
            {
                var mousePosition = Mouse.GetState().Position;
                var tile = GetTileSelectionFromMouseLocation( mousePosition );
                _parent.CurrentlySelectedTile = tile;
                Shown = false;
            }

            if( Mouse.GetState().LeftButton == ButtonState.Released )
            {
                mousePreviouslyPressed = false;
            }
        }

        private TileSheetTile GetTileSelectionFromMouseLocation( Point mousePosition )
        {
            var scaledTileSizeWidth = ScaledWidth / TotalTileX;
            var scaledTileSizeHeight = ScaledHeight / TotalTileY;

            var tileX = mousePosition.X / scaledTileSizeWidth;
            var tileY = ( mousePosition.Y + Math.Abs( imageScrollValue ) ) / scaledTileSizeHeight;

            return new TileSheetTile( CurrentlyLoadedTileSet, tileX, tileY );
        }

        private void ScrollImageUpIfPossible()
        {
            if( imageScrollValue < 0 )
                imageScrollValue += scrollIncrement;
        }

        private void ScrollImageDownIfPossible()
        {
            if( Math.Abs( imageScrollValue ) + Globals.GameSettings.GameHeight < ScaledHeight )
                imageScrollValue -= scrollIncrement;
        }

        public void Draw( GameTime gameTime )
        {
            _graphicDevice.Clear( Color.Black );
            _spriteBatch.Begin( samplerState: SamplerState.PointClamp );
            _spriteBatch.Draw( TileSetTarget, new Rectangle( 0, imageScrollValue, ScaledWidth, ScaledHeight ), Color.White );
            _spriteBatch.End();
        }
    }
}

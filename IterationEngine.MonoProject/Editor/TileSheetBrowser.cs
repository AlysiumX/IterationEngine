using IterationEngine.MonoProject.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace IterationEngine.MonoProject.Editor
{
    public class TileSheetBrowser : GameState //TODO : Not really a game state.
    {
        public bool Shown { get; set; }

        private GraphicsDevice _graphicDevice { get { return Globals.GraphicsDevice; } }
        private SpriteBatch _spriteBatch { get { return Globals.SpriteBatch; } }
        private int TileSize { get { return Globals.GameSettings.TileSize; } }

        private Texture2D CurrentlyLoadedTileSet { get; set; }
        private int TotalTileX { get; set; }
        private int TotalTileY { get; set; }

        private RenderTarget2D TileSetTarget { get; set; }

        private int ScaledWidth { get; set; }
        private int ScaledHeight { get; set; }
        private float Scale { get; set; }

        private int previousScroll;
        private int imageScrollValue = 0;
        private int scrollIncrement = 35;

        private MapEditor _parent { get; set; }

        public void SetParent( MapEditor parent )
        {
            _parent = parent;
        }

        public void Initialize() { }

        public void Show( Texture2D texture )
        {
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
            var spaceCounterOffsetX = 0;
            var spaceCounterOffsetY = 0;
            var spaceBetween = 1;

            TileSetTarget = new RenderTarget2D( _graphicDevice, CurrentlyLoadedTileSet.Width + ( TotalTileX * spaceBetween ), CurrentlyLoadedTileSet.Height + ( TotalTileY * spaceBetween ) );
            _graphicDevice.SetRenderTarget( TileSetTarget );
            _spriteBatch.Begin( samplerState: SamplerState.PointClamp );
            for( var y = 0; y<TotalTileY; y++ )
            {
                spaceCounterOffsetX = 0;
                for( var x = 0; x<TotalTileX; x++ )
                {
                    _spriteBatch.Draw( CurrentlyLoadedTileSet, new Rectangle( x * TileSize + spaceCounterOffsetX, y * TileSize + spaceCounterOffsetY, TileSize, TileSize ), new Rectangle( x * TileSize, y * TileSize, TileSize, TileSize ), Color.White );
                    spaceCounterOffsetX += spaceBetween;
                }
                spaceCounterOffsetY += spaceBetween;
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

            if( Mouse.GetState().LeftButton == ButtonState.Pressed && !Input.MousePreviouslyPress )
            {
                Input.MousePreviouslyPress = true;
                var mousePosition = Mouse.GetState().Position;
                var tile = GetTileSelectionFromMouseLocation( mousePosition );
                _parent.CurrentlySelectedTile = tile;
                Shown = false;
            }

            if( Mouse.GetState().LeftButton == ButtonState.Released )
            {
                Input.MousePreviouslyPress = false;
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

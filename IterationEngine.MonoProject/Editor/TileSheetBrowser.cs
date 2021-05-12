using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace IterationEngine.MonoProject.Editor
{
    public class TileSheetBrowser : GameState
    {
        private GraphicsDevice _graphicsDevice { get; set; }
        private SpriteBatch _spriteBatch { get; set; }

        private Texture2D CurrentlyLoadedTileSet { get; set; }

        public void Initialize( GraphicsDevice graphicsDevice, SpriteBatch spriteBatch )
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
        }

        public void Load( Texture2D texture )
        {
            CurrentlyLoadedTileSet = texture;
        }

        public void Update( GameTime gameTime )
        {
            throw new NotImplementedException();
        }

        public void Draw( GameTime gameTime )
        {
            throw new NotImplementedException();
        }
    }
}

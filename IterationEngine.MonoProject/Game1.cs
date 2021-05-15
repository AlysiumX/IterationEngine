using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IterationEngine.MonoProject
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private MapEditor _editor;

        public Game1()
        {
            Globals.Initialize();
            _graphics = new GraphicsDeviceManager( this );

            //TODO : Settings - Allow these to change during run time.
            _graphics.PreferredBackBufferWidth = Globals.GameSettings.GameWidth;
            _graphics.PreferredBackBufferHeight = Globals.GameSettings.GameHeight;
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.IsFullScreen = Globals.GameSettings.IsFullScreen;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _editor = new MapEditor();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch( GraphicsDevice );
            Globals.SetGraphicsDeviceAndSpriteBatch( _graphics.GraphicsDevice, _spriteBatch );
            _editor.Initialize();
        }

        protected override void Update( GameTime gameTime )
        {
            if( GamePad.GetState( PlayerIndex.One ).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown( Keys.Escape ) )
                Exit();

            _editor.Update( gameTime );

            base.Update( gameTime );
        }

        protected override void Draw( GameTime gameTime )
        {
            _editor.Draw( gameTime );

            base.Draw( gameTime );
        }
    }
}

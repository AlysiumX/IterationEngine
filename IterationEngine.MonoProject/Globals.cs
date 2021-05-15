using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace IterationEngine.MonoProject
{
    public static class Globals
    {
        public static GameSettings GameSettings { get; private set; }
        public static GraphicsDevice GraphicsDevice { get; private set; }
        public static SpriteBatch SpriteBatch { get; private set; }

        public static void Initialize()
        {
            GameSettings = new GameSettings();
            GameSettings.CreateSettingsFileIfNotExists();
            GameSettings.LoadSettings();
        }

        public static void SetGraphicsDeviceAndSpriteBatch( GraphicsDevice graphicsDevice, SpriteBatch spriteBatch )
        {
            GraphicsDevice = graphicsDevice;
            SpriteBatch = spriteBatch;
        }
    }
}

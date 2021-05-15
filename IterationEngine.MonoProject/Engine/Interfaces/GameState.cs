using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace IterationEngine.MonoProject
{
    public interface GameState
    {
        void Initialize();
        void Update( GameTime gameTime );
        void Draw( GameTime gameTime );
    }
}

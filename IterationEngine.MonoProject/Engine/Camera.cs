using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Text;

namespace IterationEngine.MonoProject
{
    public class Camera
    {
        private GraphicsDevice _graphicsDevice;

        private float zoomUpperLimit = 3.0f;
        private float zoomLowerLimit = .5f;

        private float _zoom;
        private Matrix _transform;
        private Vector2 _pos;
        private float _rotation;
        private int _viewportWidth;
        private int _viewportHeight;
        private int _worldWidth;
        private int _worldHeight;

        private int previousScroll;
        private float zoomIncrement = 0.2f;

        public bool IsAtLeftBarrier { get; private set; }
        public bool IsAtRightBarrier { get; private set; }
        public bool IsAtTopBarrier { get; private set; }
        public bool IsAtBottomBarrier { get; private set; }

        public Camera( GraphicsDevice graphicsDevice, Viewport viewport, Vector2 startingPosition,
        int worldWidth, int worldHeight, float initialZoom )
        {
            _graphicsDevice = graphicsDevice;

            _zoom = initialZoom;
            _rotation = 0.0f;
            _pos = startingPosition;
            _viewportWidth = viewport.Width;
            _viewportHeight = viewport.Height;
            _worldWidth = worldWidth;
            _worldHeight = worldHeight;
        }

        #region Properties

        public void SetZoomLimitsBasedOnTileSize( int tileSize )
        {
            switch( tileSize )
            {
                case TileSizes.Small:
                    zoomUpperLimit = 3.0f;
                    zoomLowerLimit = .5f;
                    break;
                case TileSizes.Medium:
                    zoomUpperLimit = 3.0f;
                    zoomLowerLimit = .5f;
                    break;
                case TileSizes.Large:
                    zoomUpperLimit = 1.0f;
                    zoomLowerLimit = .2f;
                    break;
            }


        }

        public float Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;
                if( _zoom < zoomLowerLimit )
                    _zoom = zoomLowerLimit;
                if( _zoom > zoomUpperLimit )
                    _zoom = zoomUpperLimit;
            }
        }

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public void Move( Vector2 amount )
        {
            _pos += amount;
        }

        public Vector2 Pos
        {
            get { return _pos; }
            set
            {
                IsAtLeftBarrier = false;
                IsAtRightBarrier = false;
                IsAtTopBarrier = false;
                IsAtBottomBarrier = false;

                float leftBarrier = (float)_viewportWidth *
                       .5f / _zoom;
                float rightBarrier = _worldWidth -
                       (float)_viewportWidth * .5f / _zoom;
                float topBarrier = _worldHeight -
                       (float)_viewportHeight * .5f / _zoom;
                float bottomBarrier = (float)_viewportHeight *
                       .5f / _zoom;
                _pos = value;
                if( _pos.X < leftBarrier )
                {
                    _pos.X = leftBarrier;
                    IsAtLeftBarrier = true;
                }

                if( _pos.X > rightBarrier )
                {
                    _pos.X = rightBarrier;
                    IsAtRightBarrier = true;
                }

                if( _pos.Y > topBarrier )
                {
                    _pos.Y = topBarrier;
                    IsAtTopBarrier = true;
                }

                if( _pos.Y < bottomBarrier )
                {
                    _pos.Y = bottomBarrier;
                    IsAtBottomBarrier = true;
                }
            }
        }

        #endregion

        public void SetWorldSize( int worldWidth, int worldHeight )
        {
            _worldWidth = worldWidth;
            _worldHeight = worldHeight;
        }

        public Matrix GetTransformation()
        {
            _transform =
               Matrix.CreateTranslation( new Vector3( -_pos.X, -_pos.Y, 0 ) ) *
               Matrix.CreateRotationZ( Rotation ) *
               Matrix.CreateScale( new Vector3( Zoom, Zoom, 1 ) ) *
               Matrix.CreateTranslation( new Vector3( _viewportWidth * 0.5f,
                   _viewportHeight * 0.5f, 0 ) );

            return _transform;
        }

        public Vector2 GetMousePosition()
        {
            // Transform mouse input from view to world position
            Matrix inverse = Matrix.Invert( GetTransformation() );
            Vector2 mousePos = Vector2.Transform( new Vector2( Mouse.GetState().X, Mouse.GetState().Y ), inverse );
            return mousePos;
        }

        public void Update( GameTime gameTime )
        {
            // Adjust zoom if the mouse wheel has moved
            if( Mouse.GetState().ScrollWheelValue > previousScroll )
                Zoom += zoomIncrement;
            else if( Mouse.GetState().ScrollWheelValue < previousScroll )
                Zoom -= zoomIncrement;

            previousScroll = Mouse.GetState().ScrollWheelValue;

            // Move the camera when the arrow keys are pressed
            Vector2 movement = Vector2.Zero;
            Viewport vp = _graphicsDevice.Viewport;

            if( Keyboard.GetState().IsKeyDown( Keys.A ) )
                movement.X--;
            if( Keyboard.GetState().IsKeyDown( Keys.D ) )
                movement.X++;
            if( Keyboard.GetState().IsKeyDown( Keys.W ) )
                movement.Y--;
            if( Keyboard.GetState().IsKeyDown( Keys.S ) )
                movement.Y++;

            Pos += movement * 20;
        }
    }
}

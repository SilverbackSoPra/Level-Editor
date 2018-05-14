using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevelEditor.Engine;
using LevelEditor.Engine.Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LevelEditor
{
    class CameraHandler
    {

        private readonly Camera mCamera;

        private Vector2 mRotation;

        private bool mLeftMouseButtonPressed = false;
        private Vector2 mLastMousePosition;

        public float mSensibility;
        public float mReactivity;
        
        public CameraHandler(Camera camera, float sensibility, float reactivity)
        {
            mCamera = camera;

            mRotation = mCamera.mRotation;

            mSensibility = sensibility;
            mReactivity = reactivity;

        }

        public void Update(float deltatime)
        {

            var mouseState = Mouse.GetState();

            var mousePosition = new Vector2(mouseState.X, mouseState.Y);

            if (mouseState.LeftButton == ButtonState.Pressed && !mLeftMouseButtonPressed)
            {

                mLastMousePosition = mousePosition;

                mLeftMouseButtonPressed = true;

            }
            else if (mouseState.LeftButton != ButtonState.Pressed && mLeftMouseButtonPressed)
            {
                mLeftMouseButtonPressed = false;
            }

            if (mLeftMouseButtonPressed)
            {

                mRotation +=
                    new Vector2(-(mLastMousePosition.X - mousePosition.X), mLastMousePosition.Y - mousePosition.Y) *
                    mSensibility * 0.001f;

                mLastMousePosition = mousePosition;

            }

            float progress = Math.Min(Math.Max(mReactivity * deltatime / 16.0f, 0.0f), 1.0f);

            mCamera.mRotation = MathExtension.Mix(mCamera.mRotation, mRotation, progress);

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {

                mCamera.mLocation += mCamera.Direction * deltatime / 100.0f;

            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {

                mCamera.mLocation -= mCamera.Direction * deltatime / 100.0f;

            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {

                mCamera.mLocation += mCamera.Right * deltatime / 100.0f;

            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {

                mCamera.mLocation -= mCamera.Right * deltatime / 100.0f;

            }

        }

    }
}

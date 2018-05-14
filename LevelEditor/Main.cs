﻿using System;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using LevelEditor.Engine;
using LevelEditor.Engine.Mesh;
using LevelEditor.Engine.Renderer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LevelEditor
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        private readonly GraphicsDeviceManager mGraphics;

        private readonly Camera mCamera;
        private readonly Scene mScene;
        private readonly Light mLight;

        private RenderTarget mRenderTarget;

        private MasterRenderer mMasterRenderer;

        private Terrain mTerrain;

        private CameraHandler mCameraHandler;

        public Main()
        {

            mGraphics = new GraphicsDeviceManager(this) { GraphicsProfile = GraphicsProfile.HiDef, PreferredBackBufferWidth = 1280, PreferredBackBufferHeight = 720 };
            
            mCamera = new Camera(fieldOfView: 55.0f, aspectRatio: 2.0f, nearPlane: 1.0f, farPlane: 400.0f);
            mCamera.UpdatePerspective();

            mCamera.mLocation = new Vector3(0.0f, 0.0f, -10.0f);

            mScene = new Scene();
            mLight = new Light();

            mLight.mAmbient = 0.1f;
            mLight.mLocation = new Vector3(1.0f, 1.0f, 0.0f) * 2000.0f;

            mCameraHandler = new CameraHandler(mCamera, 2.0f, 0.3f);

            mScene.Add(mLight);

            IsMouseVisible = true;

            Content.RootDirectory = "Content";
          
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {


            // TODO: use this.Content to load your game content here
            mRenderTarget = new RenderTarget(mGraphics.GraphicsDevice, 1920, 1080, 4096);

            mMasterRenderer = new MasterRenderer(mGraphics.GraphicsDevice, Content);

            
            mTerrain = new Terrain(Content, "heightmap", "shitty-grass", mGraphics.GraphicsDevice);
            var terrainActor = new Actor(mTerrain.mMesh);
            mScene.Add(terrainActor);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mCameraHandler.Update(gameTime.ElapsedGameTime.Milliseconds);

            // TODO: Add your update logic here
            mCamera.UpdateView();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            var time = gameTime.TotalGameTime.TotalMilliseconds / 4000.0f;

            // Rotate the light
            mLight.mLocation = new Vector3((float)Math.Sin(time), 1.0f, (float)Math.Cos(time)) * 1000.0f;

            mMasterRenderer.Render(mRenderTarget, mCamera, mScene);

            base.Draw(gameTime);

        }
    }
}

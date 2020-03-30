using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ParticleSystemStarter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        FireBall fireBall;
        Rain rainSystem;

        // Non-particle system based game objects

        Rectangle floor;
        Texture2D floorTexture;

        Random random = new Random();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            int floorHeight = 50;
            floor = new Rectangle(0, GraphicsDevice.Viewport.Height - floorHeight, GraphicsDevice.Viewport.Width, floorHeight);
            floorTexture = Content.Load<Texture2D>("pixel");

            Texture2D rainTexture = Content.Load<Texture2D>("particle");
            rainSystem = new Rain(GraphicsDevice.Viewport.Width, GraphicsDevice, 1500, rainTexture);
            rainSystem.SpawnPerFrame = 2;


            Texture2D fireBallTexture = Content.Load<Texture2D>("particle");
            fireBall = new FireBall(GraphicsDevice, 100, fireBallTexture);
            fireBall.SpawnPerFrame = 4;
            fireBall.Emitter = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
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


            // if mouse click, convert fire ball state to fall then explode
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Console.WriteLine("Mouse Left Button Pressed:");
                fireBall.Drop(floor.Y);
            }

            fireBall.Update(gameTime);
            rainSystem.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            rainSystem.Draw();
            spriteBatch.Begin();
            spriteBatch.Draw(floorTexture, floor, Color.Black);
            spriteBatch.End();
            fireBall.Draw();

            base.Draw(gameTime);
        }
    }
}

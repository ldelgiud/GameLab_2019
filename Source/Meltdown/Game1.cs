using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.Entities;

using Meltdown.Components;
using Meltdown.Systems;
using System;

namespace Meltdown
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        World world;

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
            spriteBatch = new SpriteBatch(GraphicsDevice);

            world = new WorldBuilder()
                .AddSystem(new TextureSystem(this.spriteBatch))
                .AddSystem(new PhysicsSystem())
                .Build();

            // Player
            var player = world.CreateEntity();
            player.Attach(new PositionComponent(new Vector2(0, 0)));
            player.Attach(new VelocityComponent(new Vector2(10, 10)));
            player.Attach(new TextureComponent(this.Content.Load<Texture2D>("player")));
            player.Attach(new EnergyComponent(100f));


            //Spawn the Nuclear Plant
            int R = 1000; // Distance from player spawn point
            Random random = new Random();
            double angle = random.NextDouble()*Math.PI/2.0;
            var plant = world.CreateEntity();
            double x = R * Math.Cos(angle);
            double y = R * Math.Sin(angle);
            plant.Attach(new PositionComponent(new Vector2((float)x,(float)y)));
            plant.Attach(new TextureComponent(this.Content.Load<Texture2D>("player")));
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            

            // TODO: use this.Content to load your game content here
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
            this.world.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            this.world.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}

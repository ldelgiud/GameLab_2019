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
            graphics.PreferredBackBufferWidth = 1000;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 1000;   // set this value to the desired height of your window
            graphics.ApplyChanges();
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
            
            //DATA to initialize playing field
            Vector2 playerPos = new Vector2(0, 900);
            double startingEnergy = 1000.0;

            //Add energy related systems to World
            PowerPlant powerPlant = new PowerPlant(900.0, playerPos);
            Energy energy = new Energy(startingEnergy);
            //Create World
            world = new WorldBuilder()
                .AddSystem(new TextureSystem(this.spriteBatch))
                .AddSystem(new PhysicsSystem())
                .AddSystem(new EnergySystem(energy, powerPlant))
                .AddSystem(new EnergyDrawSystem(energy, 
                    this.Content.Load<Texture2D>("EnergyBar"), 
                    spriteBatch,
                    Content.Load<SpriteFont>("EnergyFont")))
                .Build();

            // Player
            var player = world.CreateEntity();
           
            player.Attach(new PositionComponent(playerPos));
            player.Attach(new VelocityComponent(new Vector2(10, -10)));
            player.Attach(new TextureComponent(this.Content.Load<Texture2D>("player")));
            player.Attach(new PlayerComponent());

            
            

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

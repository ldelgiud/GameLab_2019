using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.Entities;

using Meltdown.Game_Elements;
using Meltdown.Components;
using Meltdown.Systems;
using System;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using MonoGame.Extended.Collisions;

namespace Meltdown
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        Quadtree quadtree;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        PowerPlant powerPlant;

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
            
            //Data to initialize playing field
            int amountOfPlayers = 1;
            List<PlayerInfo> playerInfos = new List<PlayerInfo>();
            //Generate Nuclear Plant object
            powerPlant = new PowerPlant();
            //Generate Shared Energy object
            Energy energy = new Energy();
            
            //Create World
            world = new WorldBuilder()
                .AddSystem(new TextureSystem(this.spriteBatch))
                .AddSystem(new PhysicsSystem(quadtree))
                .AddSystem(new EnergySystem(energy, powerPlant))
                .AddSystem(new EnergyDrawSystem(energy, 
                    this.Content.Load<Texture2D>("EnergyBar"), 
                    spriteBatch,
                    Content.Load<SpriteFont>("EnergyFont")))
                .AddSystem(new PlayerUpdateSystem())
                .AddSystem(new PlayerInfoSystem(playerInfos))
                .AddSystem(new AISystem(playerInfos))
                .Build();
            

            //Spawn player(s)
            for (int i = 0; i < amountOfPlayers; ++i)
            {
                playerInfos.Add(
                    SpawnHelper.SpawnPLayer(world, Content, i));
            }

            //Spawn powerplant
            SpawnHelper.SpawnNuclearPowerPlant(world, Content, powerPlant);
            //Spawn one enemy for testing purposes
            SpawnHelper.SpawEnemy(world, Content, new Vector2(50, 650));
            base.Initialize();
            //Spawn one battery 
            SpawnHelper.SpawnBattery(world, Content, Constants.BIG_BATTERY_SIZE, new Vector2(300, 300));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Safety check
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Escape)) Exit();

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
            //TODO: understand where (what code section) to actually draw powerplant
            

            this.world.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}

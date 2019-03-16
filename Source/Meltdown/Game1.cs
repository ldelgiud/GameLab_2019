using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


using Meltdown.Game_Elements;
using Meltdown.Components;
using Meltdown.Systems;
using System;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

using Meltdown.State;
using Meltdown.States;

using Nez;

namespace Meltdown
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Core
    {
        public static Game1 Instance { get; private set; }
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        public Game1() : base(width: 1280, height: 768, isFullScreen: false, enableEntitySystems: false)
        { }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            Window.AllowUserResizing = true;

            // create our Scene with the DefaultRenderer and a clear color of CornflowerBlue
            var myScene = Scene.createWithDefaultRenderer(Color.CornflowerBlue);

            List<PlayerInfo> playerInfos = new List<PlayerInfo>();
            //Generate Nuclear Plant object
            PowerPlant powerPlant = new PowerPlant();
            //Generate Shared Energy object
            Energy energy = new Energy();

            myScene.addEntityProcessor(new EnergySystem(
                    energy,
                    powerPlant,
                    new Matcher().all(typeof(PlayerComponent), typeof(PositionComponent))));

            myScene.addEntityProcessor(new PlayerUpdateSystem( new Matcher().all(
            typeof(PlayerComponent),
            typeof(PositionComponent),
            typeof(VelocityComponent)
            )));

            myScene.addEntityProcessor(new PlayerInfoSystem(playerInfos,
                new Matcher().all(typeof(PlayerComponent), typeof(PositionComponent))));



            myScene.addEntityProcessor(new AISystem(
                    playerInfos, new Matcher().all(
                        typeof(AIComponent),
                        typeof(PositionComponent),
                        typeof(VelocityComponent))));
            // set the scene so Nez can take over

            //Data to initialize playing field
            int amountOfPlayers = 1;



            //Spawn player(s)
            for (int i = 0; i < amountOfPlayers; ++i)
            {
                playerInfos.Add(
                    SpawnHelper.SpawnPLayer(myScene, i));
            }

            //Spawn powerplant
            SpawnHelper.SpawnNuclearPowerPlant(powerPlant, myScene);
            //Spawn one enemy for testing purposes
            SpawnHelper.SpawEnemy(new Vector2(50, 650), myScene);
            //Spawn one battery 
            SpawnHelper.SpawnBattery(Constants.BIG_BATTERY_SIZE,
                new Vector2(300, 300), myScene);

            scene = myScene;

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            IState initialState = new MainMenuState();
            initialState.Initialize(this);
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

            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}

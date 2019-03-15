using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Meltdown.State;
using Meltdown.Game_Elements;
using MonoGame.Extended.Entities;
using Meltdown.Systems;
using MonoGame.Extended.Collisions;
using MonoGame.Extended;

namespace Meltdown.States
{
    class GameState : IState
    {
        Game1 game;
        SpriteBatch spriteBatch;
        public World world;
        public Quadtree quadtree;


        public void Destroy()
        {
            throw new NotImplementedException();
        }

        public void Draw(GameTime gameTime)
        {
            this.world.Draw(gameTime);
        }

        public void Initialize(Game game)
        {
            quadtree = new Quadtree(new RectangleF(
                new Vector2(0,0), new Size2(1000f, 1000f)));
            this.game = (Game1)game;
            spriteBatch = new SpriteBatch(this.game.GraphicsDevice);
            PowerPlant powerPlant;

            //Data to initialize playing field
            int amountOfPlayers = 1;
            List<PlayerInfo> playerInfos = new List<PlayerInfo>();
            //Generate Nuclear Plant object
            powerPlant = new PowerPlant();
            //Generate Shared Energy object
            Energy energy = new Energy();

            //Create World
            this.world = new WorldBuilder()
                .AddSystem(new TextureSystem(this.spriteBatch))
                .AddSystem(new PhysicsSystem(
                    quadtree))
                .AddSystem(new EnergySystem(
                    energy, 
                    powerPlant))
                .AddSystem(new EnergyDrawSystem(
                    energy,
                    this.game.Content.Load<Texture2D>("EnergyBar"),
                    spriteBatch,
                    this.game.Content.Load<SpriteFont>("EnergyFont")))
                .AddSystem(new PlayerUpdateSystem())
                .AddSystem(new PlayerInfoSystem(
                    playerInfos))
                .AddSystem(new AISystem(
                    playerInfos))
                .Build();


            //Spawn player(s)
            for (int i = 0; i < amountOfPlayers; ++i)
            {
                playerInfos.Add(
                    SpawnHelper.SpawnPLayer(i));
            }

            //Spawn powerplant
            SpawnHelper.SpawnNuclearPowerPlant(powerPlant);
            //Spawn one enemy for testing purposes
            SpawnHelper.SpawEnemy(new Vector2(50, 650));
            //Spawn one battery 
            SpawnHelper.SpawnBattery(Constants.BIG_BATTERY_SIZE, 
                new Vector2(300, 300));
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        public void Suspend()
        {
            throw new NotImplementedException();
        }

        public IStateTransition Update(GameTime gameTime)
        {
            this.world.Update(gameTime);
            return null;
        }
    }
}

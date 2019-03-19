using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Resource;

using Meltdown.State;
using Meltdown.Systems;
using Meltdown.Components;
using Meltdown.ResourceManagers;

namespace Meltdown.States
{
    class GameState : State.State
    {
        World world;
        ISystem<GameTime> updateSystem;
        ISystem<GameTime> drawSystem;

        TextureResourceManager textureResourceManager;

        public override void Initialize(Game game)
        {
            Game1 hazmat = (Game1)game;

            this.world = new World();
            this.textureResourceManager = new TextureResourceManager(hazmat.Content);


            this.updateSystem = new SequentialSystem<GameTime>(
                new PhysicsSystem(this.world)
                );

            this.drawSystem = new SequentialSystem<GameTime>(
                new TextureDrawSystem(this.world, hazmat.spriteBatch)
                );


            // Resource Managers
            this.textureResourceManager.Manage(this.world);

            // Create player
            {
                var entity = this.world.CreateEntity();
                entity.Set(new PositionComponent() { x = 0, y = 0 });
                entity.Set(new VelocityComponent() { dx = 10, dy = 10 });
                entity.Set(new ManagedResource<string, Texture2D>("player"));
            }

        }

        public override IStateTransition Update(GameTime gameTime)
        {
            this.updateSystem.Update(gameTime);
            return null;
        }

        public override void Draw(GameTime gameTime)
        {
            this.drawSystem.Update(gameTime);
        }
    }
}

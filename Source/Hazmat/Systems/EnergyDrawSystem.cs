using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Resource;

using Hazmat.Components;
using Hazmat.Graphics;
using Hazmat.Utilities;

namespace Hazmat.Systems
{
    class EnergyDrawSystem : ISystem<Time>, IDisposable
    {

        public bool IsEnabled { get; set; } = true;

        Energy energy;

        Entity energyBarEntity;
        Entity energyBarBackgroundEntity;

        public EnergyDrawSystem(Energy energy, World world)
        {
            this.energy = energy;

            {
                var entity = world.CreateEntity();

                entity.Set(new ScreenSpaceComponent());
                entity.Set(new Transform2DComponent(new Transform2D(new Vector2(0, -520))));
                entity.Set(new ManagedResource<Texture2DInfo, AtlasTextureAlias>(new Texture2DInfo(@"static_sprites/SPT_UI_HUD_EnergyBack")));

                this.energyBarBackgroundEntity = entity;
            }

            {
                var entity = world.CreateEntity();

                entity.Set(new ScreenSpaceComponent());
                entity.Set(new Transform2DComponent(new Transform2D(new Vector2(0, -520))));
                entity.Set(new ManagedResource<Texture2DInfo, AtlasTextureAlias>(new Texture2DInfo(@"static_sprites/SPT_UI_HUD_EnergyFront")));

                this.energyBarEntity = entity;
            }
        }

        public void Update(Time gameTime)
        {
            ref var transform = ref this.energyBarEntity.Get<Transform2DComponent>();

            // TODO: Shift to proper location

            // Scale to proper size
            transform.value.Scale = new Vector2((float)(this.energy.CurrentEnergy / Constants.PLAYER_INITIAL_ENERGY), 1);
        }

        public void Dispose()
        {
        }
    }
}

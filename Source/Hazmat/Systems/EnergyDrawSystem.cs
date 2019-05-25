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
        static float BAR_WIDTH = 600;

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
                entity.Set(new Transform2DComponent(new Transform2D(new Vector2(0, -510))));
                entity.Set(new ManagedResource<Texture2DInfo, AtlasTextureAlias>(new Texture2DInfo(@"static_sprites/SPT_UI_HUD_EnergyBack", scale: new Vector2(1, 2))));

                this.energyBarBackgroundEntity = entity;
            }

            {
                var entity = world.CreateEntity();

                entity.Set(new ScreenSpaceComponent());
                entity.Set(new Transform2DComponent(new Transform2D(new Vector2(0, -510))));
                entity.Set(new ManagedResource<Texture2DInfo, AtlasTextureAlias>(new Texture2DInfo(@"static_sprites/SPT_UI_HUD_EnergyFront", scale: new Vector2(1, 2))));

                this.energyBarEntity = entity;
            }
        }

        public void Update(Time gameTime)
        {
            var ratio = (float)(this.energy.CurrentEnergy / Constants.PLAYER_INITIAL_ENERGY);

            ref var transform = ref this.energyBarEntity.Get<Transform2DComponent>();

            // TODO: Shift to proper location
            var translation = transform.value.LocalTranslation;
            transform.value.LocalTranslation = new Vector2(-BAR_WIDTH / 2 + BAR_WIDTH / 2 * ratio, translation.Y);

            // Scale to proper size
            transform.value.Scale = new Vector2(ratio, 1);
        }

        public void Dispose()
        {
        }
    }
}

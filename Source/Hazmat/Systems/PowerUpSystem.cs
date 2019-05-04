using System;
using DefaultEcs;
using DefaultEcs.System;

using Microsoft.Xna.Framework.Input;

using Hazmat.Components;
using Hazmat.Utilities;
using Hazmat.Input;
using Hazmat.Utilities.Extensions;

namespace Hazmat.Systems
{
    class PowerUpSystem : AEntitySystem<Time>
    {
        World world;
        float currentShowTime;

        public PowerUpSystem(World world) : base(
            world.GetEntities()
            .With<DisplayPowerUpChoiceComponent>()
            .Build())
        {
            this.world = world;
        }

        protected override void Update(Time time, in Entity entity)
        {
            ref DisplayPowerUpChoiceComponent displayPowerUp = ref entity.Get<DisplayPowerUpChoiceComponent>();



            // Displaying power up choice
            if (displayPowerUp.CurrentShowTime < displayPowerUp.ShowTime)
            {
                displayPowerUp.IncrementCurrentShowTime(time.Delta);
            }
            else
            {
                DisablePowerUpDisplay(entity);
            }
        }


        void DisablePowerUpDisplay(Entity entity)
        {
            entity.Delete();
        }

    }
}

using System;

using Microsoft.Xna.Framework;

using DefaultEcs;

using Hazmat.Components;
using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;

using DefaultEcs.System;

namespace Hazmat.Systems
{
    class PowerplantSystem : ISystem<Time>, IDisposable
    {
        Energy energy;
        EntitySet players;
        PowerPlant powerPlant;
        const int minDist = 400;

        public bool IsEnabled { get; set; } = true;

        public PowerplantSystem(World world, Energy energy, PowerPlant powerPlant)
        {
            this.energy = energy;
            this.powerPlant = powerPlant;
            this.players = world.GetEntities().With<PlayerComponent>().Build();

        }


        public void Update(Time gameTime)
        {
            Vector2 center = new Vector2(0,0); 
            foreach (Entity entity in this.players.GetEntities())
            {
                center += entity.Get<Transform3DComponent>().value.Translation.ToVector2();
            }
            center /= players.Count;
            Vector2 distVec = center - powerPlant.Position;
            double dist = Math.Max(distVec.Length(), minDist);

            this.energy.CurrentEnergy -= (1 / dist) * gameTime.Delta * 1000;


            }

        public void Dispose()
        {
        }
    }
    
}

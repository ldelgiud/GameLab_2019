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
        const int minDist = 100;
        const int maxDist = 1000;

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
            double dist = Math.Min(Math.Max(distVec.Length(), minDist),maxDist);
            //This allows for a drain between 1 and 10 scaling linearly with the distance
            double decrease = (maxDist - (dist - minDist))/100;

            this.energy.CurrentEnergy -= decrease * gameTime.Delta;


            }

        public void Dispose()
        {
        }
    }
    
}

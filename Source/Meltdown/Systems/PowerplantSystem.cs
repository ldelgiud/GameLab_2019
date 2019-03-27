using System;

using Microsoft.Xna.Framework;

using DefaultEcs;

using Meltdown.Components;
using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;

using DefaultEcs.System;

namespace Meltdown.Systems
{
    class PowerplantSystem : ISystem<Time>, IDisposable
    {
        Energy energy;
        EntitySet players;
        PowerPlant powerPlant;
        const int minDist = 20;

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
                center += entity.Get<WorldTransformComponent>().value.position.ToVector2();
            }
            center /= players.Count;
            Vector2 distVec = center - powerPlant.Position;
            double dist = distVec.Length();

            this.energy.CurrentEnergy -= (1 / dist) * gameTime.Delta * 10000;


            }

        public void Dispose()
        {
        }
    }
    
}

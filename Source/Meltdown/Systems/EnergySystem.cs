using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Meltdown.Components;
using Microsoft.Xna.Framework;
using Meltdown;

using Nez;

namespace Meltdown.Systems
{
    class EnergySystem : EntityProcessingSystem
    {
        
        PowerPlant powerPlant;
        Energy energy;
        const int minDist = 100;
        public EnergySystem(
            Energy energy, 
            PowerPlant powerPlant,
            Matcher matcher) : base( matcher )
        {
            this.energy = energy;
            this.powerPlant = powerPlant;
        }
        

        public override void process(Entity entity)
        {

           
            var position = entity.getComponent<PositionComponent>();

            double squaredDist = Math.Max(Math.Pow((powerPlant.Position.X - position.position.X), 2)
                 + Math.Pow(powerPlant.Position.Y - position.position.Y, 2),
                minDist);
            
            //TODO: Delta time used incorrectly, understand it better
            this.energy.CurrentEnergy -= (1/Math.Sqrt(squaredDist)) * ((float)Time.deltaTime / 1000f) *10000;
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

using Meltdown.Components;
using Microsoft.Xna.Framework;
using Meltdown;

namespace Meltdown.Systems
{
    class EnergySystem : EntityUpdateSystem
    {
        ComponentMapper<PositionComponent> positionMapper;
        PowerPlant powerPlant;
        Energy energy;
        const int minDist = 100;
        public EnergySystem(Energy energy, PowerPlant powerPlant) : base(Aspect.All(typeof(PositionComponent), 
            typeof(PlayerComponent)))
        {
            this.energy = energy;
            this.powerPlant = powerPlant;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            this.positionMapper = mapperService.GetMapper<PositionComponent>();
        }

        public override void Update(GameTime gameTime)
        {

            foreach (int id in this.ActiveEntities)
            {
                PositionComponent position = this.positionMapper.Get(id);

                double squaredDist = Math.Max(Math.Pow((powerPlant.Position.X - position.position.X), 2)
                    + Math.Pow(powerPlant.Position.Y - position.position.Y, 2),
                    minDist);
                
                this.energy.energy -= (1/Math.Sqrt(squaredDist)) * ((float)gameTime.ElapsedGameTime.Milliseconds / 1000f) *10000;


            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

using Meltdown.Components;
using Microsoft.Xna.Framework;

namespace Meltdown.Systems
{
    class EnergySystem : EntityUpdateSystem
    {
        ComponentMapper<EnergyComponent> EnergyMapper;


        public EnergySystem() : base(Aspect.All(typeof(EnergyComponent)))
        {

        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            this.EnergyMapper = mapperService.GetMapper<EnergyComponent>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (int id in this.ActiveEntities)
            {
                //TODO: Add Logic behind energyBar
            }
        }
    }
}

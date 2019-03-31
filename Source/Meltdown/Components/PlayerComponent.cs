using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meltdown.Components
{
    public struct PlayerComponent
    {
        public int Id { get; } 

        public PlayerComponent(int id)
        {
            this.Id = id;
        }
    }
}

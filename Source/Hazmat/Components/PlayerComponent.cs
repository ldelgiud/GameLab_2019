using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hazmat.Components
{
    public struct PlayerComponent
    {
        public float Speed { get; set; }
        public int Id { get; } 

        public PlayerComponent(int id, float speed)
        {
            this.Id = id;
            this.Speed = speed;
        }
        
    }
}

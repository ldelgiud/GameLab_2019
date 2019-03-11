using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meltdown.Components
{
    class PlayerComponent
    {
        public int Id { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">starts at 0 and linearly increase, do not give random values</param>
        public PlayerComponent(int id)
        {
            this.Id = id;
        }
    }
}

using DefaultEcs.System;
using Meltdown.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meltdown.Pathfinding;
namespace Meltdown.Systems
{
    class PathFinderSystem : ISystem<Time>, IDisposable
    {
        PathRequestManager PathRequestManager
        {
            get
            {
                return Game1.Instance.ActiveState.GetInstance<PathRequestManager>();
            }
        }
        public bool IsEnabled { get; set; } = true;
        public PathFinderSystem()
        {

        }
        public void Dispose()
        {
           
        }

        public void Update(Time state)
        {
            PathRequestManager.TryProcessNext();
        }
    }
}

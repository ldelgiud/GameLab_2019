using DefaultEcs.System;
using Hazmat.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hazmat.Pathfinding;
namespace Hazmat.Systems
{
    class PathFinderSystem : ISystem<Time>, IDisposable
    {
        PathRequestManager PathRequestManager
        {
            get
            {
                return Hazmat.Instance.ActiveState.GetInstance<PathRequestManager>();
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

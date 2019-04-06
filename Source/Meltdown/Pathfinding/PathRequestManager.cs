using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meltdown.Pathfinding
{
    class PathRequestManager
    {
        Queue<PathRequest> pathRequestQueue;
        PathRequest currentPathRequest;
        PathFinder pathFinder;

        bool isProcessingPath;

        public PathRequestManager(PathFinder pathFinder)
        {
            this.pathRequestQueue =  new Queue<PathRequest>();
            this.pathFinder = pathFinder;
        }
        
        public void RequestPath(Vector2 start, Vector2 end, Action<Vector2[], bool> callback)
        {
            PathRequest newRequest = new PathRequest(start, end, callback);
            this.pathRequestQueue.Enqueue(newRequest);
            this.TryProcessNext();
        }

        void TryProcessNext()
        {
            if (!isProcessingPath && pathRequestQueue.Count > 0)
            {
                this.currentPathRequest = pathRequestQueue.Dequeue();
                this.isProcessingPath = true;
                pathFinder.FindPath(this.currentPathRequest.start, this.currentPathRequest.end);
            }
        }

        public void FinishedProcessingPath(Vector2[] path, bool success)
        {
            this.currentPathRequest.callback(path, success);
            this.isProcessingPath = false;
            TryProcessNext();
        }

        struct PathRequest
        {
            public Vector2 start, end;
            public Action<Vector2[], bool> callback;

            public PathRequest(Vector2 start, Vector2 end, Action<Vector2[], bool> callback)
            {
                this.start = start;
                this.end = end;
                this.callback = callback;
            }

        }
    }
}

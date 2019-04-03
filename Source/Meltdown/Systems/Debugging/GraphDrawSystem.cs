using DefaultEcs.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Meltdown.Utilities;
using Meltdown.Pathfinding;
using Microsoft.Xna.Framework.Graphics;

namespace Meltdown.Systems.Debugging
{
    class GraphDrawSystem : ISystem<Time>, IDisposable
    {

        Grid grid;
        SpriteBatch spriteBatch;

        public bool IsEnabled { get; set; } = true;

        public GraphDrawSystem(Grid grid, SpriteBatch spriteBatch)
        {
            this.grid = grid;
            this.spriteBatch = spriteBatch;
        }

        public void Update(Time state)
        {
            int gridX = grid.GridSizeX;
            int gridY = grid.GridSizeY;

            spriteBatch.Begin();
            for (int y = 0; y < gridY; ++y)
            {
                for (int x = 0; x < gridX; ++x)
                {
                    switch(grid.Nodes[y,x].walkable)
                    {
                        case true:

                            break;
                        case false:

                            break;
                    }
                        
                }
            }
            spriteBatch.End();

        }

        public void Dispose()
        {
        }
    }
}

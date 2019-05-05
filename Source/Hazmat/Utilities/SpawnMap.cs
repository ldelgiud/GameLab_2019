using Hazmat.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hazmat.Utilities
{
    class SpawnMap
    {

        public double[][] map;

        public SpawnMap()
        {
            int xSize = (int) (Constants.RIGHT_BORDER / (Constants.TILE_SIZE * 10));
            int ySize = (int) (Constants.TOP_BORDER / (Constants.TILE_SIZE * 10));
            this.map = new double[ySize][];

            for (int i = 0; i < ySize; i++)
            {
                map[i] = new double[xSize];
            }
        }
    }
}

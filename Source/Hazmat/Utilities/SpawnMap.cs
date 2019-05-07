using Hazmat.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hazmat.Utilities
{
    class SpawnMap
    {

        double[][] map;
        public int tilesize = (int) Constants.TILE_SIZE * 10;
        public int xSize, ySize; 

        public SpawnMap()
        {
            this.xSize = (int)((Constants.RIGHT_BORDER - Constants.LEFT_BORDER) / this.tilesize);
            this.ySize = (int)((Constants.TOP_BORDER - Constants.BOTTOM_BORDER) / this.tilesize);
            this.map = new double[ySize][];

            for (int i = 0; i < ySize; i++)
            {
                map[i] = new double[xSize];
            }
        }

        public double GetSpawnRate(Vector2 position)
        {
            Tuple<int,int> index = this.VectorToIndex(position);
            return this.map[index.Item1][index.Item2];
        }

        public void SetSpawnRate(Vector2 position, double rate)
        {
            Tuple<int, int> index = this.VectorToIndex(position);
            Debug.Assert(rate >= 0 && rate <= 1);

            this.map[index.Item1][index.Item2] = 
                Math.Max(rate, this.map[index.Item1][index.Item2]);
        }

        private Tuple<int,int> VectorToIndex (Vector2 vec)
        {
            vec -= Constants.BOTTOM_LEFT_CORNER;
            int x = (int)(vec.X / this.tilesize);
            int y = (int)(vec.Y / this.tilesize);

            Debug.Assert(x < this.xSize && y < this.ySize);

            return new Tuple<int,int>(y,x);
        }
    }
}

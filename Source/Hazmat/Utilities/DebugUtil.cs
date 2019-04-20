using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Hazmat.Utilities
{
    public static class DebugUtil
    {

        public static void PrintMatrix(Matrix m)
        {
            for (int i = 0; i < 4; i++)
            {
                string row = "[ ";
                for (int j = 0; j < 4; j++)
                {
                    row += m[i * 4 + j].ToString("0.0") + " ";
                }
                row += "]";
                Debug.WriteLine(row);
            }
        }
    }
}

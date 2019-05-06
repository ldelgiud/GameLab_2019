using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Collision;

namespace Hazmat.Utilities.Extensions
{
    public static class QuadtreeExtension
    {
        public static void MyRayCast<T>(this QuadTree<T> quadTree, 
            Func<RayCastInput, Element<T>, float> callback, 
            ref RayCastInput input)
        {
            Stack<QuadTree<T>> stack = new Stack<QuadTree<T>>();
            stack.Push(quadTree);

            float maxFraction = input.MaxFraction;
            Vector2 p1 = input.Point1;
            Vector2 p2 = p1 + (input.Point2 - input.Point1) * maxFraction;

            while (stack.Count > 0)
            {
                QuadTree<T> qt = stack.Pop();

                if (!QuadTree<T>.RayCastAABB(qt.Span, p1, p2))
                    continue;

                foreach (Element<T> n in qt.Nodes)
                {
                    if (!QuadTree<T>.RayCastAABB(n.Span, p1, p2))
                        continue;

                    RayCastInput subInput;
                    subInput.Point1 = input.Point1;
                    subInput.Point2 = input.Point2;
                    subInput.MaxFraction = maxFraction;

                    float value = callback(subInput, n);
                    if (value == 0.0f)
                        return; // the client has terminated the raycast.

                    if (value <= 0.0f)
                        continue;

                    maxFraction = value;
                    p2 = p1 + (input.Point2 - input.Point1) * maxFraction; //update segment endpoint
                }
                if (qt.IsPartitioned)
                    foreach (QuadTree<T> st in qt.SubTrees)
                        stack.Push(st);
            }
        }
    }
}

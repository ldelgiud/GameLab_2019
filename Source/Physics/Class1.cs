using System;

namespace Physics
{
    struct Point {
        float x;
        float y;

        Point(float x, float y) {
            this.x = x;
            this.y = y;
        }
    }
    //Axis Aligned Bunding Box
    
    struct AABB
    {
        Point center;
        float width, height;

        AABB(Point center, float width, float height) {
            this.center = center;
            this.width = width;
            this.height = height;
        }
        bool containsPoint(Point point) {
            return false;
        }
        bool intersectsAABB(AABB other) {
            return false;
        }
    }
    public class Quadtree
    {
        AABB boundary;
        Quadtree NW, NE, SE, SW;

        Quadtree(AABB boundary)
        {
            this.boundary = boundary;
        }

        
    }
}

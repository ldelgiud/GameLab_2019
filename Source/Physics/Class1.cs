using System;
using System.Collections.Generic;
using ECS;

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
        public bool ContainsPoint(Point point) {
            return false;
        }
        public bool ContainsAABB(AABB other)
        {
            return false;
        }
        public bool IntersectsAABB(AABB other) {
            return false;
        }
    }
    public class Quadtree
    {
        public const int DefaultMaxDepth = 7;
        public const int DefaultMaxObjectsPerNode = 25;
        AABB boundary;
        Quadtree NW, NE, SE, SW;
        //AT the moment all entities store MUST have an AABB (no point insertions)
        List<Entity> objects = new List<Entity>();

        Quadtree(AABB boundary)
        {
            this.boundary = boundary;
        }

        bool Insert(Context ctx, Entity ent) 
        {
            //check if AABB is cotained in this quadtree
            if (!boundary.ContainsAABB(ctx.GetComponent<AABB>(ent))) return false;

            //if this quadtree has children
            if (NW != null)
            {
                //try recursively inserting in children
                if (NW.Insert(ctx, ent)) return true;
                if (NE.Insert(ctx, ent)) return true;
                if (SE.Insert(ctx, ent)) return true;
                if (SW.Insert(ctx, ent)) return true;
            }

            //We know the entity is contained, so we can insert it here

            return true;
        }
    }
}

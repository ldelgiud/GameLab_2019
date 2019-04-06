using DefaultEcs.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Meltdown.Utilities;
using Meltdown.Pathfinding;
using Microsoft.Xna.Framework.Graphics;
using Meltdown.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Meltdown.Systems.Debugging
{
    class GraphDrawSystem : ISystem<Time>, IDisposable
    {

        Grid grid;
        SpriteBatch spriteBatch;
        Camera2D camera;
        Texture2D circle;

        public bool IsEnabled { get; set; } = true;

        public GraphDrawSystem(Grid grid, GraphicsDevice graphicsDevice, Camera2D camera, Texture2D circle) { 
            this.grid = grid;
            this.spriteBatch = new SpriteBatch(graphicsDevice);
            this.camera = camera;
            this.circle = circle;
        }

        public void Update(Time state)
        {
            this.spriteBatch.Begin();

            Vector2 start = this.camera.Transform.Translation - new Vector2(this.camera.ViewportWidth / 2, this.camera.ViewportHeight / 2);
            if (this.grid.VectorToNode(start) == null) return;

            int startX = this.grid.VectorToNode(start).gridX;
            int startY = this.grid.VectorToNode(start).gridY;

            int EndX = startX + (int) MathF.Round(camera.ViewportWidth / grid.NodeRadius);
            int EndY = startY + (int)MathF.Round(camera.ViewportWidth / grid.NodeRadius);
            Vector2 size = new Vector2(grid.NodeRadius);

            
            for (int y = startY; y < EndY; ++y)
            {
                for (int x = startX; x < EndX; ++x)
                {
                    Transform2D transform = new Transform2D(grid.Nodes[y, x].WorldPosition);
                    var (position, rotation, scale) = 
                        this.camera.ToScreenCoordinates(transform, new Texture2DInfo(null, scale: size / this.circle.Bounds.Size.ToVector2()));
                    switch (grid.Nodes[y,x].walkable)
                    {
                        case true:
                            spriteBatch.Draw(
                                circle,
                                position: position,
                                rotation: rotation,
                                scale: scale,
                                color: Color.DarkBlue
                            );
                            break;
                        case false:
                            spriteBatch.Draw(
                                circle,
                                position: position,
                                rotation: rotation,
                                scale: scale,
                                color: Color.Red
                            );
                            break;
                    }
                        
                }
            }
            this.spriteBatch.End();
        }

        public void Dispose()
        {
        }
    }
}

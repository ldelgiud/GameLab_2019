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
        Texture2D blueCircle;
        Texture2D redCircle;

        public bool IsEnabled { get; set; } = true;

        public GraphDrawSystem(Grid grid, GraphicsDevice graphicsDevice, Camera2D camera, Texture2D blueCircle, Texture2D redCircle)
        {
            this.grid = grid;
            this.spriteBatch = new SpriteBatch(graphicsDevice);
            this.camera = camera;
            this.blueCircle = blueCircle;
            this.redCircle = redCircle;
        }

        public void Update(Time state)
        {
            this.spriteBatch.Begin();

            int gridX = grid.GridSizeX;
            int gridY = grid.GridSizeY;
            Vector2 size = new Vector2(grid.NodeRadius/2);

            for (int y = 0; y < gridY; ++y)
            {
                for (int x = 0; x < gridX; ++x)
                {
                    //TODO: make this shit actually draw 
                    Transform2D transform = new Transform2D(grid.Nodes[y, x].WorldPosition);
                    var (position, rotation, scale) = 
                        this.camera.ToScreenCoordinates(transform, new Texture2DInfo(null, scale: size / this.blueCircle.Bounds.Size.ToVector2()));
                    Debug.WriteLine("node position: " + grid.Nodes[y, x].WorldPosition);
                    switch (grid.Nodes[y,x].walkable)
                    {
                        case true:
                            spriteBatch.Draw(
                                blueCircle,
                                position: position,
                                rotation: rotation,
                                scale: scale,
                                color: Color.DarkBlue
                            );
                            Debug.WriteLine("walkable");
                            break;
                        case false:
                            spriteBatch.Draw(
                                redCircle,
                                position: position,
                                rotation: rotation,
                                scale: scale,
                                color: Color.Red
                            );
                            Debug.WriteLine("NOT walkable");
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

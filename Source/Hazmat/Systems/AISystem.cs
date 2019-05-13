using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DefaultEcs;
using DefaultEcs.System;

using Hazmat.Components;
using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;
using Hazmat.Graphics;
using tainicom.Aether.Physics2D.Collision;

namespace Hazmat.Systems
{
    class AISystem : ISystem<Time>, IDisposable
    {
        public bool IsEnabled { get; set; } = true;
        EntitySet players;
        Camera3D camera;
        public AISystem(World world, Camera3D camera) /*: base(
            world.GetEntities()
            .With<Transform3DComponent>()
            .With<WorldSpaceComponent>()
            .With<AIComponent>()
            .Build())*/
        {
            this.players = world.GetEntities().With<PlayerComponent>().Build();
            this.camera = camera;
        }

        public void Update(Time state)
        {
            List<PlayerInfo> playerInfos = new List<PlayerInfo>();

            foreach (Entity entity in this.players.GetEntities())
            {
                playerInfos.Add(new PlayerInfo(
                    entity.Get<Transform3DComponent>().value,
                    entity.Get<PlayerComponent>().Id));
            }


            AABB cameraAABB = this.camera.ViewBounds;
            cameraAABB.LowerBound -= Vector2.One * 20;
            cameraAABB.UpperBound += Vector2.One * 20;
            List<Entity> entities = new List<Entity>();
            
            SpawnHelper.quadtree.QueryAABB((element) =>
            {
                var entity = element.Value;
                if (entity.Has<AIComponent>())
                {
                    entities.Add(entity);
                    
                }
                return true;
            }, ref cameraAABB);

            foreach (Entity ent in entities)
            {
                ref AIComponent aIState = ref ent.Get<AIComponent>();
                aIState.State =
                    aIState.State.UpdateState(playerInfos, state);
            }
        }

        public void Dispose()
        {
        }
    }
}

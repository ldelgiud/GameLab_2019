using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;

using Meltdown.Collision;
using Meltdown.Components;
using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;
using Meltdown.Graphics;
using Meltdown.Components.InputHandlers;
using DefaultEcs.Resource;

namespace Meltdown.Collision.Handlers
{
    class PowerUpPickUpCollisionHandler : CollisionHandler
    {
        World world;

        public PowerUpPickUpCollisionHandler(World world) : base(
            new Type[] { typeof(PlayerComponent), typeof(AABBComponent) },
            new Type[] { typeof(PowerUpComponent), typeof(AABBComponent) }
            )
        {
            this.world = world;
        }

        public override void HandleCollision(CollisionType type, Entity collider, Entity collidee)
        {
            ref Transform2DComponent playerTransform = ref collider.Get<Transform2DComponent>();
            ref StatsComponent playerStats = ref collider.Get<StatsComponent>();

            // Attach a DisplayPowerUpChoiceComponent
            Entity entity = this.world.CreateEntity();
            entity.Set(playerStats);
            entity.Set(new DisplayPowerUpChoiceComponent(Constants.POWERUP_DISPLAY_TIME));
            entity.Set(new InputComponent(inputHandler: new PowerUpInputHandler(this.world)));
            entity.Set(new Transform2DComponent(new Transform2D(position: new Vector2(7f, 7f), parent: playerTransform.value)));
            entity.Set(new WorldSpaceComponent());
            entity.Set(new ManagedResource<SpineAnimationInfo, SkeletonDataAlias>(
               new SpineAnimationInfo(
                   @"menu\main\screens",
                   new SkeletonInfo(10f, 7f, skin: "upgrade_popup"),
                   new AnimationStateInfo("upgrade_popup", false)
               )
           ));

            collidee.Delete();
        }
    }
}

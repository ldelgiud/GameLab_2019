using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;

using Hazmat.Collision;
using Hazmat.Components;
using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;
using Hazmat.Graphics;
using Hazmat.Components.InputHandlers;
using DefaultEcs.Resource;

namespace Hazmat.Collision.Handlers
{
    class PowerUpPickUpCollisionHandler : CollisionHandler
    {
        World world;
        Score score;

        public PowerUpPickUpCollisionHandler(World world, Score score) : base(
            new Type[] { typeof(PlayerComponent), typeof(Transform3DComponent), typeof(AABBComponent) },
            new Type[] { typeof(PowerUpComponent), typeof(Transform3DComponent), typeof(AABBComponent) }
            )
        {
            this.world = world;
            this.score = score;
        }

        public override void HandleCollision(CollisionType type, Entity collider, Entity collidee)
        {
            ref Transform3DComponent playerTransform = ref collider.Get<Transform3DComponent>();
            ref StatsComponent playerStats = ref collider.Get<StatsComponent>();

            // Attach a DisplayPowerUpChoiceComponent
            Entity entity = this.world.CreateEntity();
            entity.Set(playerStats);
            entity.Set(new DisplayPowerUpChoiceComponent(Constants.POWERUP_DISPLAY_TIME));
            entity.Set(new InputComponent(inputHandler: new PowerUpInputHandler(this.world, this.score)));
            entity.Set(new Transform3DComponent(new Transform3D(position: new Vector3(0f, 0f, 7f), parent: playerTransform.value)));
            entity.Set(new WorldSpaceComponent());
            entity.Set(new ManagedResource<SpineAnimationInfo, SkeletonDataAlias>(
               new SpineAnimationInfo(
                   @"ui\SPS_Screens",
                   new SkeletonInfo(10f, 7f, skin: "upgrade_popup"),
                   new AnimationStateInfo("upgrade_popup", false)
               )
           ));

            collidee.Delete();
        }
    }
}

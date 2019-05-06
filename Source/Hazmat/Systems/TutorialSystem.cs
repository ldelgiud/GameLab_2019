using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using DefaultEcs;
using DefaultEcs.System;

using tainicom.Aether.Physics2D.Collision;

using Hazmat.Components;
using Hazmat.Event;
using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;

namespace Hazmat.Systems
{
    enum Tutorial
    {
        FirstSteps,
        AtPowerPlant,
        BatteryPickup,
        Enemy,
        Lootbox,
        OffPath,
        Weapon
    }

    class TutorialSystem : ISystem<Time>, IDisposable
    {
        PowerPlant powerPlant;
        World world;
        QuadTree<Entity> quadtree;

        public bool IsEnabled { get; set; }

        HashSet<Tutorial> tutorialComplete = new HashSet<Tutorial>();

        EntitySet players;

        public TutorialSystem(World world, PowerPlant powerPlant)
        {
            this.powerPlant = powerPlant;
            this.world = world;
            this.quadtree = Hazmat.Instance.ActiveState.GetInstance<QuadTree<Entity>>();

            this.players = world.GetEntities()
                .With<PlayerComponent>()
                .With<Transform3DComponent>().Build();
        }

        public void Update(Time time)
        {
            if (!this.tutorialComplete.Contains(Tutorial.AtPowerPlant))
            {
                foreach (var playerEntity in this.players.GetEntities())
                {
                    ref var transform = ref playerEntity.Get<Transform3DComponent>();

                    if (Vector2.Distance(powerPlant.Position, transform.value.Translation.ToVector2()) <= 40)
                    {
                        var entity = this.world.CreateEntity();
                        var _event = new TipEvent("tip_at_the_powerplant");
                        _event.Initialize(this.world, entity);
                        entity.Set(new EventComponent(_event));
                        entity.Set(new NameComponent() { name = "event" });
                        this.tutorialComplete.Add(Tutorial.AtPowerPlant);
                    }
                }
            }

            if (!this.tutorialComplete.Contains(Tutorial.BatteryPickup))
            {
                foreach (var playerEntity in this.players.GetEntities())
                {
                    ref var aabbRef = ref playerEntity.Get<AABBComponent>();
                    var aabb = aabbRef.element.Span;
                    aabb.UpperBound += new Vector2(10, 10);
                    aabb.LowerBound -= new Vector2(10, 10);

                    this.quadtree.QueryAABB((element) =>
                    {
                        if (element.Value.Has<EnergyPickupComponent>() || element.Value.Has<PowerUpComponent>())
                        {
                            var entity = this.world.CreateEntity();
                            var _event = new TipEvent("tip_battery_powerup");
                            _event.Initialize(this.world, entity);
                            entity.Set(new EventComponent(_event));
                            entity.Set(new NameComponent() { name = "event" });
                            this.tutorialComplete.Add(Tutorial.BatteryPickup);

                            return false;
                        }

                        return true;
                    }, ref aabb);
                }
            }

            if (!this.tutorialComplete.Contains(Tutorial.Enemy))
            {
                foreach (var playerEntity in this.players.GetEntities())
                {
                    ref var aabbRef = ref playerEntity.Get<AABBComponent>();
                    var aabb = aabbRef.element.Span;
                    aabb.UpperBound += new Vector2(20, 20);
                    aabb.LowerBound -= new Vector2(20, 20);

                    this.quadtree.QueryAABB((element) =>
                    {
                        if (element.Value.Has<AIComponent>())
                        {
                            var entity = this.world.CreateEntity();
                            var _event = new TipEvent("tip_first_enemy");
                            _event.Initialize(this.world, entity);
                            entity.Set(new EventComponent(_event));
                            entity.Set(new NameComponent() { name = "event" });
                            this.tutorialComplete.Add(Tutorial.Enemy);

                            return false;
                        }

                        return true;
                    }, ref aabb);
                }
            }

            if (!this.tutorialComplete.Contains(Tutorial.Lootbox))
            {
                foreach (var playerEntity in this.players.GetEntities())
                {
                    ref var aabbRef = ref playerEntity.Get<AABBComponent>();
                    var aabb = aabbRef.element.Span;
                    aabb.UpperBound += new Vector2(10, 10);
                    aabb.LowerBound -= new Vector2(10, 10);

                    this.quadtree.QueryAABB((element) =>
                    {
                        if (element.Value.Has<LootableComponent>())
                        {
                            var entity = this.world.CreateEntity();
                            var _event = new TipEvent("tip_lootbox");
                            _event.Initialize(this.world, entity);
                            entity.Set(new EventComponent(_event));
                            entity.Set(new NameComponent() { name = "event" });
                            this.tutorialComplete.Add(Tutorial.Lootbox);

                            return false;
                        }

                        return true;
                    }, ref aabb);
                }
            }

            if (!this.tutorialComplete.Contains(Tutorial.Weapon))
            {
                foreach (var playerEntity in this.players.GetEntities())
                {
                    ref var aabbRef = ref playerEntity.Get<AABBComponent>();
                    var aabb = aabbRef.element.Span;
                    aabb.UpperBound += new Vector2(5, 5);
                    aabb.LowerBound -= new Vector2(5, 5);

                    this.quadtree.QueryAABB((element) =>
                    {
                        if (element.Value.Has<SmallGunComponent>() && element.Value.Has<InteractableComponent>())
                        {
                            var entity = this.world.CreateEntity();
                            var _event = new TipEvent("tip_weapon");
                            _event.Initialize(this.world, entity);
                            entity.Set(new EventComponent(_event));
                            entity.Set(new NameComponent() { name = "event" });
                            this.tutorialComplete.Add(Tutorial.Weapon);

                            return false;
                        }

                        return true;
                    }, ref aabb);
                }
            }


        }

        public void Dispose()
        {

        }

    }
}

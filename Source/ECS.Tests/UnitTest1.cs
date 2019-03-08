using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using ECS;
using ECS.Storages;

namespace Tests
{
    public class Tests
    {
        struct PositionComponent
        {
            public float x;
            public float y;

            public List<UInt32> list;

            public override string ToString()
            {
                return String.Format("PositionComponent {{ x: {0}, y: {1} }}", this.x, this.y);
            }
        }

        struct VelocityComponent
        {
            public float dx;
            public float dy;

            public override string ToString()
            {
                return String.Format("VelocityComponent {{ dx: {0}, dy: {1} }}", this.dx, this.dy);
            }
        }

        struct InputComponent
        {
            public int ctr;

            public override string ToString()
            {
                return String.Format("InputComponent {{ {0} }}", this.ctr);
            }
        }

        class PhysicsSystem : ECS.System<ConcreteRef<PositionComponent>, ConcreteRef<VelocityComponent>>
        {
            public PhysicsSystem() : base(new Type[] { typeof(SetPositionAction) }) { }

            public override void Tick(ActionStore actionStore, Entity entity, ConcreteRef<PositionComponent> positionRef, ConcreteRef<VelocityComponent> velocityRef)
            {
                var action = new SetPositionAction();
                action.entity = entity;
                action.x = positionRef.Value.x + velocityRef.Value.dx;
                action.y = positionRef.Value.y + velocityRef.Value.dy;

                actionStore.Add(action);
            }
        }

        struct SetPositionAction : IAction
        {
            public Entity entity;
            public float x;
            public float y;

            public void Apply(Context ctx)
            {
                ref PositionComponent position = ref ctx.GetComponent<PositionComponent>(this.entity);

                position.x = this.x;
                position.y = this.y;
            }

            public override String ToString()
            {
                return String.Format("SetPositionAction {{ entity: {0}, x: {1}, y: {2} }}", this.entity, this.x, this.y);
            }
        }

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {
            var dispatcher = Dispatcher.Builder()
                .Add(new PhysicsSystem(), new Type[] { })
                .Build();

            Context ctx = new Context();
            ctx.Register(new SparseStorage<PositionComponent>());
            ctx.Register(new SparseStorage<VelocityComponent>());
            ctx.Register(new UnitStorage<InputComponent>());


            InputComponent input = new InputComponent();
            ctx.AddUnitComponent<InputComponent>(input);
            {
                Entity entity = ctx.CreateEntity();
                PositionComponent position = new PositionComponent();
                position.x = 1.0f;
                position.y = 1.0f;
                ctx.AddComponent(entity, position);
                VelocityComponent velocity = new VelocityComponent();
                velocity.dx = 0.1f;
                velocity.dy = 0.2f;
                ctx.AddComponent(entity, velocity);
            }

            {
                Entity entity = ctx.CreateEntity();
                PositionComponent position = new PositionComponent();
                position.x = 2.0f;
                position.y = 2.0f;
                ctx.AddComponent(entity, position);
            }

            for (int i = 0; i < 10; ++i)
            {
                Console.WriteLine("Tick!");
                dispatcher.Tick(ctx);
                Console.WriteLine(ctx);
            }

            Assert.Fail();
        }
    }
}
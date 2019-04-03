using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Diagnostics;

using DefaultEcs;
using DefaultEcs.System;

using Meltdown.Components;
using Meltdown.Utilities;
using Meltdown.Graphics;
using Meltdown.Utilities.Extensions;
using Meltdown.States;
using Microsoft.Xna.Framework.Input;

namespace Meltdown.Systems
{
    sealed class TextureEffectsDrawSystem : AEntitySystem<Time>
    {
        Camera camera;
        SpriteBatch spriteBatch;
        Effect effect;

        public TextureEffectsDrawSystem(GraphicsDevice graphicsDevice, Camera camera, World world, Effect effect) : base(
            world.GetEntities()
            .With<WorldTransformComponent>()
            .With<TextureEffectComponent>()
            .Build())
        {
            this.camera = camera;
            this.spriteBatch = new SpriteBatch(graphicsDevice);
            this.effect = effect;
        }

        protected override void PreUpdate(Time time)
        {
           // effect.Parameters["time"].SetValue(time.Absolute);
            this.spriteBatch.Begin(effect: this.effect);
        }

        protected override void Update(Time time, in Entity entity)
        {
            ref WorldTransformComponent transform = ref entity.Get<WorldTransformComponent>();
            var transformMatrix = transform.value.GlobalTransform;

            ref TextureEffectComponent texture = ref entity.Get<TextureEffectComponent>();
            var (position, rotation, scale, origin) = this.camera.ToScreenCoordinates(transformMatrix, texture.value.Bounds);

            // Override scale to correct shear
            scale = transform.value.scale.ToVector2();

            this.spriteBatch.Draw(
                texture: texture.value,
                position: position,
                rotation: rotation,
                scale: scale,
                origin: origin
                );


            // DEBUG: draw origin
            var circleScale = Vector2.One * 0.15f;
            this.spriteBatch.Draw(
                texture: Game1.Instance.Content.Load<Texture2D>("circle-64"),
                position: position,
                scale: circleScale,
                origin: Vector2.One * 32,
                color: Color.DeepPink
                );
        }

        protected override void PostUpdate(Time time)
        {
            this.spriteBatch.End();
        }
    }
}

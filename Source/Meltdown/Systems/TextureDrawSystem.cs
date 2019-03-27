﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.System;

using Meltdown.Components;
using Meltdown.Utilities;
using Meltdown.Graphics;
using Meltdown.Utilities.Extensions;


namespace Meltdown.Systems
{
    sealed class TextureDrawSystem : AEntitySystem<Time>
    {
        GameWindow window;
        Camera camera;
        SpriteBatch spriteBatch;

        public TextureDrawSystem(GameWindow window, GraphicsDevice graphicsDevice, Camera camera, World world) : base(
            world.GetEntities()
            .With<WorldTransformComponent>()
            .With<BoundingBoxComponent>()
            .WithAny<TextureAnimateComponent, TextureComponent>()
            .Build())
        {
            this.window = window;
            this.camera = camera;
            this.spriteBatch = new SpriteBatch(graphicsDevice);
        }

        protected override void PreUpdate(Time time)
        {
            this.spriteBatch.Begin();
        }

        protected override void Update(Time time, in Entity entity)
        {
            ref WorldTransformComponent transform = ref entity.Get<WorldTransformComponent>();
            ref BoundingBoxComponent bounds = ref entity.Get<BoundingBoxComponent>();

            var transformMatrix = transform.value.GlobalTransform;

            // Invert 

            if (entity.Has<TextureComponent>())
            {
                ref TextureComponent texture = ref entity.Get<TextureComponent>();

                

                var (position, rotation, scale, origin) = this.camera.ToScreenCoordinates(this.window.ClientBounds, transformMatrix, texture.value.Bounds);

                // Override scale to correct shear
                scale = transform.value.scale.ToVector2();

                this.spriteBatch.Draw(
                    texture: texture.value,
                    position: position,
                    rotation: rotation,
                    scale: scale,
                    origin: origin
                    );

            }
            else
            {
                ref TextureAnimateComponent textureAnim = ref entity.Get<TextureAnimateComponent>();
                textureAnim.UpdateAnimation(time.Delta);
                var source = new Rectangle(textureAnim.currentFrame * textureAnim.frameWidth + 1, 0, textureAnim.frameWidth, textureAnim.frameHeight);

                var (position, rotation, scale, origin) = this.camera.ToScreenCoordinates(this.window.ClientBounds, transformMatrix, source);

                // Override scale to correct shear
                scale = transform.value.scale.ToVector2();

                this.spriteBatch.Draw(
                    texture: textureAnim.texture,
                    sourceRectangle: source,
                    position: position,
                    rotation: rotation,
                    scale: scale,
                    origin: origin
                    );

            }
        }

        protected override void PostUpdate(Time time)
        {
            this.spriteBatch.End();
        }
    }
}

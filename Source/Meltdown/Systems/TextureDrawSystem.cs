using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

using DefaultEcs;
using DefaultEcs.System;

using Meltdown.Components;
using Meltdown.Utilities;

namespace Meltdown.Systems
{
    sealed class TextureDrawSystem : AEntitySystem<Time>
    {
        Camera camera;
        SpriteBatch spriteBatch;

        public TextureDrawSystem(GraphicsDevice graphicsDevice, Camera camera, World world) : base(
            world.GetEntities()
            .With<WorldTransformComponent>()
            .With<BoundingBoxComponent>()
            .WithAny<TextureAnimateComponent, TextureComponent>()
            .Build())
        {
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

            // Coarse Culling
            if (!camera.Intersects(transform.Position, bounds.value)) {
                return;
            }

            if (entity.Has<TextureComponent>())
            {
                ref TextureComponent texture = ref entity.Get<TextureComponent>();
                this.spriteBatch.Draw(
                    texture: texture.texture,
                    destinationRectangle: this.camera.Project(transform.Position, bounds.value),
                    rotation: transform.Rotation,
                    scale: transform.Scale
                    );
            }
            else
            {
                ref TextureAnimateComponent textureAnim = ref entity.Get<TextureAnimateComponent>();
                textureAnim.UpdateAnimation(time.Delta);
                this.spriteBatch.Draw(
                    texture: textureAnim.texture,
                    sourceRectangle: new Rectangle(textureAnim.currentFrame * textureAnim.frameWidth + 1, 0, textureAnim.frameWidth, textureAnim.frameHeight),
                    destinationRectangle: this.camera.Project(transform.Position, bounds.value),
                    rotation: transform.Rotation,
                    scale: transform.Scale
                    );

                //Debug.WriteLine("Animate frames: " + textureAnim.nrFrames +
                //    ", timeToChangeSprite: " + textureAnim.timeChangeSprite +
                //    ", timeWithCurrentSprite: " + textureAnim.timeWithCurrentSprite +
                //    ", width: " + textureAnim.frameWidth +
                //    ", height " + textureAnim.frameHeight +
                //    ", currentFrame: " + textureAnim.currentFrame
                //    );
            }       
        }

        protected override void PostUpdate(Time time)
        {
            this.spriteBatch.End();
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

using DefaultEcs;
using DefaultEcs.System;


using Meltdown.Components;
using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;


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
               // transform.Rotation += 0.01f;
                this.spriteBatch.Draw(
                    texture: texture.value,
                    destinationRectangle: this.camera.Project(transform.Position, texture.value.Bounds),
                    rotation: transform.Rotation,
                    scale: transform.Scale,
                    origin: new Vector2((bounds.value.Width() / 2) * camera.WidthRatio, (bounds.value.Height() / 2) * camera.HeightRatio)
                    );

            }
            else
            {
                ref TextureAnimateComponent textureAnim = ref entity.Get<TextureAnimateComponent>();
                textureAnim.UpdateAnimation(time.Delta);
                var source = new Rectangle(textureAnim.currentFrame * textureAnim.frameWidth + 1, 0, textureAnim.frameWidth, textureAnim.frameHeight);
                this.spriteBatch.Draw(
                    texture: textureAnim.texture,
                    sourceRectangle: source,
                    destinationRectangle: this.camera.Project(transform.Position, source),
                    rotation: transform.Rotation,
                    scale: transform.Scale,
                    origin: new Vector2(bounds.value.Max.X * transform.Scale.X, bounds.value.Max.Y * transform.Scale.X)
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

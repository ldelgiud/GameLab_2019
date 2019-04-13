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
    sealed class TextureDrawSystem : AEntitySystem<Time>
    {
        Camera camera;
        SpriteBatch spriteBatch;
        Effect effect;

        public TextureDrawSystem(GraphicsDevice graphicsDevice, Camera camera, World world, Effect effect) : base(
            world.GetEntities()
            .With<WorldTransformComponent>()
            .WithAny<TextureAnimateComponent, TextureComponent>()
            .Build())
        {
            this.camera = camera;
            this.spriteBatch = new SpriteBatch(graphicsDevice);
            this.effect = effect;
            effect.Parameters["u_blurSize"].SetValue(0.03f);
            effect.Parameters["u_intensity"].SetValue(2.5f);
        }

        protected override void PreUpdate(Time time)
        {
            
        }

        protected override void Update(Time time, in Entity entity)
        {
            ref WorldTransformComponent transform = ref entity.Get<WorldTransformComponent>();

            var transformMatrix = transform.value.GlobalTransform;
            
            if (entity.Has<TextureComponent>())
            {
                ref TextureComponent texture = ref entity.Get<TextureComponent>();
                var (position, rotation, scale, origin) = this.camera.ToScreenCoordinates(transformMatrix, texture.value.Bounds);

                // Override scale to correct shear
                scale = transform.value.scale.ToVector2();

                if (texture.glowing)
                {
                    this.spriteBatch.Begin(effect: this.effect);
                    
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

                  
                    this.spriteBatch.End();
                }
                else
                {
                    this.spriteBatch.Begin();
                    

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

                    this.spriteBatch.End();
                }

                
            }
            //else
            //{
            //    ref TextureAnimateComponent textureAnim = ref entity.Get<TextureAnimateComponent>();
            //    textureAnim.UpdateAnimation(time.Delta);
            //    var source = new Rectangle(textureAnim.currentFrame * textureAnim.frameWidth + 1, 0, textureAnim.frameWidth, textureAnim.frameHeight);

            //    var (position, rotation, scale, origin) = this.camera.ToScreenCoordinates(transformMatrix, source);

            //    // Override scale to correct shear
            //    scale = transform.value.scale.ToVector2();

            //    this.spriteBatch.Draw(
            //        texture: textureAnim.texture,
            //        sourceRectangle: source,
            //        position: position,
            //        rotation: rotation,
            //        scale: scale,
            //        origin: origin
            //        );

            //}
        }

        protected override void PostUpdate(Time time)
        {
           // this.spriteBatch.End();
        }
    }
}

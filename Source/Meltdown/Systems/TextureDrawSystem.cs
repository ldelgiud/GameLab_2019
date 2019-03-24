﻿using Microsoft.Xna.Framework;
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
        SpriteBatch spriteBatch;

        public TextureDrawSystem(World world, SpriteBatch spriteBatch) : base(
            world.GetEntities()
            .With<WorldTransformComponent>()
            .WithAny<TextureAnimateComponent, TextureComponent>()
            .Build())
        {
            this.spriteBatch = spriteBatch;
        }

        protected override void PreUpdate(Time time)
        {
            this.spriteBatch.Begin();
        }

        protected override void Update(Time time, in Entity entity)
        {
            ref WorldTransformComponent transform = ref entity.Get<WorldTransformComponent>();
            try
            {
                ref TextureComponent texture = ref entity.Get<TextureComponent>();
                this.spriteBatch.Draw(texture: texture.texture, position: transform.Position, 
                                      rotation: transform.Rotation, scale: transform.Scale);

            }
            catch (System.Exception){ }

            try
            {
                ref TextureAnimateComponent textureAnim = ref entity.Get<TextureAnimateComponent>();
                textureAnim.UpdateAnimation(time.Delta);
                this.spriteBatch.Draw(texture: textureAnim.texture, 
                    sourceRectangle: new Rectangle(textureAnim.currentFrame*textureAnim.frameWidth + 1, 0, textureAnim.frameWidth, textureAnim.frameHeight),
                    position: transform.Position, rotation: transform.Rotation, scale: transform.Scale);
             
            }
            catch (System.Exception) { }
            
        }

        protected override void PostUpdate(Time time)
        {
            this.spriteBatch.End();
        }
    }
}

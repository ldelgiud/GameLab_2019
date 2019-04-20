using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs.Resource;
using DefaultEcs;

using System;
using System.Diagnostics;

using Hazmat.Components;
using Hazmat.Utilities;

namespace Hazmat.ResourceManagers
{
    sealed class TextureResourceManager : AResourceManager<Texture2DInfo, Texture2DAlias>
    {
        ContentManager contentManager;

        bool animated;
        int timeChangeSprite;
        int nrFrames;
        int frameWidth;
        int frameHeight;

        public TextureResourceManager(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        protected override Texture2DAlias Load(Texture2DInfo info)
        {
            string[] infos = info.name.Split('*');
            if (infos.Length == 1) animated = false;
            else
            {
                animated = true;
                int.TryParse(infos[1], out timeChangeSprite);
                int.TryParse(infos[2], out nrFrames);
                int.TryParse(infos[3], out frameWidth);
                int.TryParse(infos[4], out frameHeight);
                if (timeChangeSprite == -1 || nrFrames == -1 || frameHeight == -1 || frameWidth == -1)
                {
                    animated = false;
                    Debug.Fail("ERROR: Failed loading an animated sprite. Check that the name formatting is as follows:" +
                        " 'textureName*timeChangeSprite*nrFrames*width*height' where * is the separator and timeChangeSprite" +
                        " is in milliseconds.");
                }
            }
            return new Texture2DAlias(this.contentManager.Load<Texture2D>(infos[0]), info.bounds);
        }

        protected override void OnResourceLoaded(in Entity entity, Texture2DInfo info, Texture2DAlias resource)
        {
            //if (animated)
            //{
            //    entity.Set(new TextureAnimateComponent(resource, animated, (float)timeChangeSprite, nrFrames, frameWidth, frameHeight));
            //}
            //else
            // Set scale from width/height
            if (info.scale.X < 0 && info.scale.Y < 0)
            {
                info.scale = new Vector2(info.width / resource.value.Bounds.Width, info.height / resource.value.Bounds.Height);
            }

            entity.Set(new Texture2DComponent(resource.value, info));
        }
    }
}

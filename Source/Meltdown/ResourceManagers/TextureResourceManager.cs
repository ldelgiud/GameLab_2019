using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs.Resource;
using DefaultEcs;

using System;
using System.Diagnostics;

using Meltdown.Components;

namespace Meltdown.ResourceManagers
{
    sealed class TextureResourceManager : AResourceManager<string, Texture2D>
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

        protected override Texture2D Load(string info)
        {
            string[] infos = info.Split('*');
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
            return this.contentManager.Load<Texture2D>(infos[0]);
        }

        protected override void OnResourceLoaded(in Entity entity, string info, Texture2D resource)
        {
            if (animated)
            {
                entity.Set(new TextureAnimateComponent(resource, animated, (float)timeChangeSprite, nrFrames, frameWidth, frameHeight));
            }
            else entity.Set(new TextureComponent() { texture = resource });
        }
    }
}

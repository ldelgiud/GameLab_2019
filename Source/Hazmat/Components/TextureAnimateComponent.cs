﻿using Microsoft.Xna.Framework.Graphics;

namespace Hazmat.Components
{
    class TextureAnimateComponent
    {
        public Texture2D texture;
        public bool animated { get; private set; }
        public float timeChangeSprite { get; private set; }
        public int nrFrames { get; private set; }
        public int frameWidth { get; private set; }
        public int frameHeight { get; private set; }
        public int currentFrame { get; private set; }
        public float timeWithCurrentSprite;


        public TextureAnimateComponent(Texture2D tex, bool anim, float timeChangeSprite, int nrFrames, int width, int height)
        {
            this.texture = tex;
            this.animated = anim;
            this.timeChangeSprite = timeChangeSprite / 1000;
            this.nrFrames = nrFrames;
            this.frameWidth = width;
            this.frameHeight = height;

            this.currentFrame = 0;
            this.timeWithCurrentSprite = 0;
        }

        public void UpdateAnimation(float passedTime)
        {
            timeWithCurrentSprite += passedTime;

            if (timeWithCurrentSprite >= timeChangeSprite)
            {
                currentFrame++;
                currentFrame = currentFrame % nrFrames;
                timeWithCurrentSprite = 0;
            }
        }

    }
}
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GmtkJam2020.Rendering
{
    public class Animator
    {
        public int Frame { get; set; }

        public float FrameDuration { get; set; }

        public Animation Animation { get; private set; }

        private float frameTimer;

        public void Update(float deltaTime)
        {
            if (frameTimer <= 0.0f)
            {
                Frame++;
                if (Animation.Frames.Count >= Frame)
                {
                    Frame = 0;
                }
            }

            frameTimer += deltaTime;
        }

        public void SetAnimation(Animation animation)
        {
            Animation = animation;
            frameTimer = 0.0f;
            Frame = 0;
        }

        public SpriteEffects GetCurrentEffect() => Animation?.Effect ?? SpriteEffects.None;

        public int GetCurrentFrame() => Animation?.Frames[Frame] ?? 0;
    }
}
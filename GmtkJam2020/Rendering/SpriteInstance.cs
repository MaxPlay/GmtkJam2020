using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmtkJam2020.Rendering
{
    public class SpriteInstance
    {
        public Sprite Source { get; set; }

        public Animator Animator { get; set; } = new Animator() { FrameDuration = 0.3f };

        public void Draw(Vector2 position)
        {
            if (Source.Texture == null)
                return;

            GameCore.Instance.SpriteBatch.Draw(Source.Texture, position - Source.Image.Pivot.ToVector2(), null, Color.White, 0.0f, Vector2.Zero, 0.0f, SpriteEffects.None, 0.0f);
        }

        public void DrawFrame(Vector2 position, string frame)
        {
            if (Source.Texture == null)
                return;

            NamedFrame namedFrame = Source.NamedFrames[frame];

            GameCore.Instance.SpriteBatch.Draw(Source.Texture, position - Source.Image.Pivot.ToVector2(), Source.GetFrame(namedFrame.Frame), Color.White, 0.0f, Vector2.Zero, 1.0f, namedFrame.Effect, 0.0f);
        }

        public void DrawAnimation(Vector2 position)
        {
            if (Source.Texture == null)
                return;

            GameCore.Instance.SpriteBatch.Draw(Source.Texture, position - Source.Image.Pivot.ToVector2(), Source.GetFrame(Animator.GetCurrentFrame()), Color.White, 0.0f, Vector2.Zero, 1.0f, Animator.GetCurrentEffect(), 0.0f);
        }
    }
}

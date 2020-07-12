using GmtkJam2020.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmtkJam2020.UI
{
    public class TextBlock
    {
        public string Text { get; set; }

        public Rectangle Bounds { get; set; }

        public void Draw()
        {
            SpriteFont spriteFont = SpriteManager.Fonts["MenuFont"];
            Vector2 fontBounds = spriteFont.MeasureString(Text);
            fontBounds.Y += 6;
            GameCore.Instance.SpriteBatch.DrawString(spriteFont, Text, Bounds.Center.ToVector2() - fontBounds / 2.0f, Color.White);
        }
    }
}

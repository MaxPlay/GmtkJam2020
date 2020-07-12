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
    public class Button
    {
        private TextBlock textBlock = new TextBlock();

        public string Text { get => textBlock.Text; set => textBlock.Text = value; }

        public Rectangle Bounds { get => textBlock.Bounds; set => textBlock.Bounds = value; }

        public int Index { get; set; }

        public void Draw(int selectedIndex)
        {
            if (selectedIndex == Index)
                GameCore.Instance.SpriteBatch.Draw(GameCore.Instance.Pixel, Bounds, new Color(Color.White, 0.3f));

            textBlock.Draw();
        }

        public Action Execute { get; set; }
    }
}

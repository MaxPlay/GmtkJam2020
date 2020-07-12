using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmtkJam2020.Rendering
{
    public static class SpriteManager
    {
        public static Dictionary<string, Sprite> Sprites { get; } = new Dictionary<string, Sprite>();

        public static Dictionary<string, SpriteFont> Fonts { get; } = new Dictionary<string, SpriteFont>();

        public static void LoadSprite(string name)
        {
            if (!Sprites.ContainsKey(name))
            {
                Sprite sprite = Sprite.Load(name);
                if (sprite != null)
                    Sprites.Add(name, sprite);
            }
        }

        public static void LoadFont(string name)
        {
            if(!Fonts.ContainsKey(name))
            {
                SpriteFont font = GameCore.Instance.Content.Load<SpriteFont>("fonts/" + name);
                if (font != null)
                    Fonts.Add(name, font);
            }
        }
    }
}

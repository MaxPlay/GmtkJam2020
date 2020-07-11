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

        public static void LoadSprite(string name)
        {
            if (!Sprites.ContainsKey(name))
            {
                Sprite sprite = Sprite.Load(name);
                if (sprite != null)
                    Sprites.Add(name, sprite);
            }
        }
    }
}

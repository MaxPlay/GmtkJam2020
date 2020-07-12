using GmtkJam2020.Gameplay;
using GmtkJam2020.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmtkJam2020.UI
{
    public class LevelUI
    {
        private List<ActionUI> elements = new List<ActionUI>();
        private SpriteInstance backgroundSprite;
        private SpriteInstance iconSprite;

        public LevelUI()
        {
            backgroundSprite = SpriteManager.Sprites["LevelUI_Background"].CreateInstance();
            iconSprite = SpriteManager.Sprites["LevelUI_Icons"].CreateInstance();
            elements = new List<ActionUI>()
            {
                new ActionUI() { Action = PlayerAction.Destroy, Frame = "Icon_Drill" },
                new ActionUI() { Action = PlayerAction.Turn, Frame = "Icon_Turn" },
                new ActionUI() { Action = PlayerAction.Push, Frame = "Icon_Push" },
                new ActionUI() { Action = PlayerAction.Pull, Frame = "Icon_Pull" },
                new ActionUI() { Action = PlayerAction.Grab, Frame = "Icon_Grapple" },
            };

            for (int i = 0; i < elements.Count; i++)
            {
                elements[i].Location = new Vector2(i * backgroundSprite.Source.Image.FrameSize.X, 160);
            }
        }

        public void Update(Player player)
        {
            elements.ForEach(e => e.Update(player));
        }

        public void Draw()
        {
            elements.ForEach(e => e.Draw(iconSprite, backgroundSprite));
        }
    }
}

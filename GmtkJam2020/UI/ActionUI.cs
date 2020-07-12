using GmtkJam2020.Gameplay;
using GmtkJam2020.Rendering;
using Microsoft.Xna.Framework;

namespace GmtkJam2020.UI
{
    public class ActionUI
    {
        public PlayerAction Action { get; set; }

        public string Frame { get; set; }

        public Vector2 Location { get; set; }

        public int Value { get; set; }

        public void Update(Player player)
        {
            Value = player.ActionDistance(Action);
        }

        public void Draw(SpriteInstance iconSprite, SpriteInstance backgroundSprite)
        {
            backgroundSprite.DrawFrame(Location, Value > 0 ? "Frame_Active" : "Frame_Disabled");
            iconSprite.DrawFrame(Location, Frame);
            GameCore.Instance.SpriteBatch.DrawString(SpriteManager.Fonts["Font"], Value > 0 ? Value.ToString() : "OoC", Location + new Vector2(24, -5), Color.White);
        }
    }
}
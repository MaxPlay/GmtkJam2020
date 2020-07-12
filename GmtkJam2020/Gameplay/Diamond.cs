using GmtkJam2020.Rendering;
using Microsoft.Xna.Framework;

namespace GmtkJam2020.Gameplay
{
    public class Diamond : LevelEntity
    {
        public Diamond(int x, int y) : base(x, y)
        {
            sprite = SpriteManager.Sprites["Diamond"].CreateInstance();
        }

        public override void Draw()
        {
            sprite.DrawFrame(new Vector2(Position.X * Level.DEFAULT_TILE_WIDTH, Position.Y * Level.DEFAULT_TILE_HEIGHT), "HoldDiamond");
        }
    }
}
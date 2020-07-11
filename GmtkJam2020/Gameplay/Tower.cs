using GmtkJam2020.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmtkJam2020.Gameplay
{
    public class Tower
    {
        public Tower(int x, int y)
        {
            Position = new Point(x, y);
            sprite = SpriteManager.Sprites["SignalTower"].CreateInstance();
            sprite.Animator.SetAnimation(sprite.Source.NamedAnimations["Idle"]);
        }

        SpriteInstance sprite;

        public Point Position { get; set; }

        public int MoveDistance { get; set; }

        public int TurnDistance { get; set; }

        public int GrabDistance { get; set; }

        public int PushDistance { get; set; }

        public int PullDistance { get; set; }

        public int DestroyDistance { get; set; }
        public Level Level { get; internal set; }

        public void Draw()
        {
            sprite.DrawAnimation(new Vector2(Position.X * Level.DEFAULT_TILE_WIDTH, Position.Y * Level.DEFAULT_TILE_HEIGHT));
        }
    }
}

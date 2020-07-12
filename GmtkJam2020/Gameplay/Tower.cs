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

        public int MoveDistance { get; set; } = int.MaxValue;

        public int TurnDistance { get; set; } = int.MaxValue;

        public int GrabDistance { get; set; } = 0;

        public int PushDistance { get; set; } = 0;

        public int PullDistance { get; set; } = 0;

        public int DestroyDistance { get; set; } = 0;

        public Level Level { get; set; }

        public void Draw()
        {
            sprite.DrawAnimation(new Vector2(Position.X * Level.DEFAULT_TILE_WIDTH, Position.Y * Level.DEFAULT_TILE_HEIGHT));
        }

        public void SetDistance(char type, int value)
        {
            switch (type)
            {
                case 'd': DestroyDistance = value; break;
                case 'm': MoveDistance = value; break;
                case 't': TurnDistance = value; break;
                case 'g': GrabDistance = value; break;
                case 'p': PushDistance = value; break;
                case 'l': PullDistance = value; break;
            }
        }
    }
}

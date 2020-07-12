using GmtkJam2020.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmtkJam2020.Gameplay
{
    public abstract class LevelEntity
    {
        public LevelEntity(int x, int y)
        {
            Position = new Point(x, y);
        }

        public Point Position { get; set; }

        public Level Level { get; set; }

        protected SpriteInstance sprite;

        public abstract void Draw();
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmtkJam2020.UI
{
    public class Button
    {
        public string Text { get; set; }

        public Rectangle Bounds { get; set; }

        public int Index { get; set; }

        public void Draw(int selectedIndex)
        {

        }

        public Action Execute { get; set; }
    }
}

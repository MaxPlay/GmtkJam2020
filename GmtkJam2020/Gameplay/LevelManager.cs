using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmtkJam2020.Gameplay
{
    public class LevelManager
    {
        public List<string> Levels { get; set; }

        public LevelManager()
        {
            string[] files = Directory.GetFiles("Content/Levels", "*.lvl");

            Levels = files.Select(f => Path.GetFileNameWithoutExtension(f)).ToList();
        }
    }
}

using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GmtkJam2020.Rendering
{
    public class Animation
    {
        public string Name { get; set; }

        public List<int> Frames { get; set; }

        public SpriteEffects Effect { get; set; }

        public bool Loops { get; set; }
    }
}
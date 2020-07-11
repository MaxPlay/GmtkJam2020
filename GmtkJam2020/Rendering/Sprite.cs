using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmtkJam2020.Rendering
{
    public class Sprite
    {
        public string Name { get; set; }

        public Texture2D Texture { get; set; }

        public Image Image { get; set; }

        private List<Rectangle> sourceFrames = new List<Rectangle>();

        public List<Animation> Animations { get; private set; } = new List<Animation>();

        public Dictionary<string, Animation> NamedAnimations { get; private set; } = new Dictionary<string, Animation>();

        public List<NamedFrame> Frames { get; private set; } = new List<NamedFrame>();

        public Dictionary<string, NamedFrame> NamedFrames { get; private set; } = new Dictionary<string, NamedFrame>();

        public Rectangle GetFrame(int frame)
        {
            if (frame >= 0 && frame < sourceFrames.Count)
                return sourceFrames[frame];
            return Texture.Bounds;
        }

        public static Sprite Load(string name)
        {
            string filename = $"Content/Sprites/{name}.json";
            if (File.Exists(filename))
            {
                string jsonData = File.ReadAllText(filename);
                Sprite sprite = JsonConvert.DeserializeObject<Sprite>(jsonData);
                sprite.Initialize(name);
                return sprite;
            }
            return null;
        }

        private void Initialize(string name)
        {
            Name = name;
            Texture = GameCore.Instance.Content.Load<Texture2D>("Sprites/" + Name);
            Animations.ForEach(a => NamedAnimations.Add(a.Name, a));
            Frames.ForEach(f => NamedFrames.Add(f.Name, f));
            int xCount = Texture.Width / Image.FrameSize.X;
            int yCount = Texture.Height / Image.FrameSize.Y;
            for (int y = 0; y < yCount; y++)
            {
                for (int x = 0; x < xCount; x++)
                {
                    sourceFrames.Add(new Rectangle(new Point(x, y) * Image.FrameSize, Image.FrameSize));
                }
            }

        }

        public SpriteInstance CreateInstance()
        {
            SpriteInstance instance = new SpriteInstance() { Source = this };
            return instance;
        }
    }
}

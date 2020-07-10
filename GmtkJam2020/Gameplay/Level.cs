using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmtkJam2020.Gameplay
{
    public class Level
    {
        private LevelTile[,] data;
        Dictionary<TileType, Color> tileColors;

        public int Width { get; }
        public int Height { get; }
        public Point TileSize { get; private set; }

        private Level(int width, int height)
        {
            Width = width;
            Height = height;
            data = new LevelTile[width, height];
            Random random = new Random();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    data[x, y] = new LevelTile() { Type = (TileType)random.Next(0, 4) };
                }
            }
            tileColors = new Dictionary<TileType, Color>()
            {
                [TileType.Floor] = Color.White,
                [TileType.Wall] = Color.DarkGray,
                [TileType.Player] = Color.Red,
                [TileType.Pushable] = Color.Green,
            };
        }

        public static Level Create(int width, int height, Point tileSize)
        {
            return new Level(width, height) { TileSize = tileSize };
        }

        public void Draw()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    GameCore.Instance.SpriteBatch.Draw(GameCore.Instance.Pixel, new Vector2(x, y) * TileSize.ToVector2(), new Rectangle(new Point(x, y), TileSize), tileColors[data[x,y].Type]);
                }
            }
        }
    }
}

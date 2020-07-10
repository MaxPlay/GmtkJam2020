using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmtkJam2020.Gameplay
{
    public class Level
    {
        const int DEFAULT_TILE_WIDTH = 16;
        const int DEFAULT_TILE_HEIGHT = 16;
        static Dictionary<char, TileType> parserMapping = new Dictionary<char, TileType>()
        {
            ['0'] = TileType.Floor,
            ['1'] = TileType.Wall,
            ['2'] = TileType.Player,
            ['3'] = TileType.Pushable,
        };

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

        public static Level LoadFromFile(string filename)
        {
            if (!File.Exists(filename))
                return new Level(0, 0);

            string[] lines = File.ReadAllLines(filename);
            if (lines.Length <= 2)
                return new Level(0, 0);

            int.TryParse(lines[0], out int width);
            int.TryParse(lines[1], out int height);

            Level level = Create(width, height, new Point(DEFAULT_TILE_WIDTH, DEFAULT_TILE_HEIGHT));
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    char value = lines[y+2][x];
                    if (parserMapping.ContainsKey(value))
                        level.data[x, y] = new LevelTile() { Type = parserMapping[value] };
                }
            }
            return level;
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
                    GameCore.Instance.SpriteBatch.Draw(GameCore.Instance.Pixel, new Vector2(x, y) * TileSize.ToVector2(), new Rectangle(new Point(x, y), TileSize), tileColors[data[x, y].Type]);
                }
            }
        }
    }
}

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
        public const int DEFAULT_TILE_WIDTH = 16;
        public const int DEFAULT_TILE_HEIGHT = 16;
        static Dictionary<char, TileType> parserMapping = new Dictionary<char, TileType>()
        {
            ['.'] = TileType.Floor,
            ['x'] = TileType.Wall,
            ['p'] = TileType.Player,
            ['3'] = TileType.Pushable,
        };

        private LevelTile[,] data;
        Dictionary<TileType, Color> tileColors;

        public int Width { get; }

        public int Height { get; }

        public Point TileSize { get; private set; }

        public Player Player { get; set; }

        private Level(int width, int height)
        {
            Width = width;
            Height = height;
            data = new LevelTile[width, height];
            tileColors = new Dictionary<TileType, Color>()
            {
                [TileType.Floor] = Color.White,
                [TileType.Wall] = Color.DarkGray,
                [TileType.Pushable] = Color.Green,
            };
        }

        public void RandomlyGenerate()
        {
            Random random = new Random();
            Player = null;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    data[x, y] = new LevelTile() { Type = (TileType)random.Next(0, 4) };
                    if (data[x, y].Type == TileType.Player)
                    {
                        if (Player == null)
                            Player = new Player(x, y);
                        data[x, y].Type = TileType.Floor;
                    }
                }
            }
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
            level.Player = null;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    char value = char.ToLowerInvariant(lines[y + 2][x]);
                    if (parserMapping.ContainsKey(value))
                    {
                        level.data[x, y] = new LevelTile() { Type = parserMapping[value] };
                        if (level.data[x, y].Type == TileType.Player)
                        {
                            if (level.Player == null)
                                level.Player = new Player(x, y);
                            level.data[x, y].Type = TileType.Floor;
                        }
                    }
                    else
                        level.data[x, y] = new LevelTile() { Type = TileType.Floor };
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
                    GameCore.Instance.SpriteBatch.Draw(GameCore.Instance.Pixel, new Vector2(x, y) * TileSize.ToVector2(), new Rectangle(new Point(), TileSize), tileColors[data[x, y].Type]);
                }
            }

            Player?.Draw();
        }
    }
}

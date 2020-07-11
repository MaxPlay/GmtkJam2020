using GmtkJam2020.Rendering;
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
        readonly Dictionary<TileType, Color> tileColors;

        private readonly static string[] floortiles = { "Floor0", "Floor1", "Floor2", "Floor3", "Floor4" };

        SpriteInstance sprite;

        public int Width { get; }

        public int Height { get; }

        public Rectangle Bounds => new Rectangle(0, 0, Width, Height);

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
            sprite = SpriteManager.Sprites["MarsTiles"].CreateInstance();
        }

        public void RandomlyGenerate()
        {
            Player = null;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    data[x, y] = new LevelTile() { Type = (TileType)GameCore.Instance.Random.Next(0, 4), Frame = GameCore.Instance.Random.Next(0, floortiles.Length) };
                    if (data[x, y].Type == TileType.Player)
                    {
                        if (Player == null)
                            Player = new Player(x, y) { Level = this };
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
                        level.data[x, y] = new LevelTile() { Type = parserMapping[value], Frame = GameCore.Instance.Random.Next(0, floortiles.Length) };
                        if (level.data[x, y].Type == TileType.Player)
                        {
                            if (level.Player == null)
                                level.Player = new Player(x, y) { Level = level };
                            level.data[x, y].Type = TileType.Floor;
                        }
                    }
                    else
                        level.data[x, y] = new LevelTile() { Type = TileType.Floor, Frame = GameCore.Instance.Random.Next(0, floortiles.Length) };
                }
            }
            return level;
        }

        public bool PushTile(Point position, Orientation direction)
        {
            if (!Bounds.Contains(position))
                return false;

            Point newPosition = position;
            switch (direction)
            {
                case Orientation.North:
                    newPosition.Y--;
                    break;

                case Orientation.West:
                    newPosition.X--;
                    break;

                case Orientation.South:
                    newPosition.Y++;
                    break;

                case Orientation.East:
                    newPosition.X++;
                    break;
            }

            if (GetTile(newPosition).Type == TileType.Floor)
            {
                data[position.X, position.Y].Type = TileType.Floor;
                data[newPosition.X, newPosition.Y].Type = TileType.Pushable;

                return true;
            }

            return false;
        }

        public LevelTile GetTile(Point point)
        {
            if (Bounds.Contains(point))
            {
                return data[point.X, point.Y];
            }
            return LevelTile.OutOfBoundsTile;
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
                    sprite.DrawFrame(new Vector2(x, y) * TileSize.ToVector2(), floortiles[data[x, y].Frame]);
                    switch (data[x, y].Type)
                    {
                        case TileType.Wall:
                            sprite.DrawFrame(new Vector2(x, y) * TileSize.ToVector2(), "Wall");
                            break;

                        case TileType.Pushable:
                            sprite.DrawFrame(new Vector2(x, y) * TileSize.ToVector2(), "Pushable");
                            break;
                    }
                }
            }

            Player?.Draw();
        }
    }
}

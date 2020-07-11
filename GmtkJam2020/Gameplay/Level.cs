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
            ['m'] = TileType.Movable,
            ['d'] = TileType.Destructible,
            ['t'] = TileType.Tower
        };

        private LevelTile[,] data;
        readonly Dictionary<TileType, Color> tileColors;

        private List<TileType> movableTiles = new List<TileType>() { TileType.Movable };
        private List<TileType> blockingTiles = new List<TileType>() { TileType.Wall, TileType.Tower, TileType.Destructible };
        private List<TileType> walkableTiles = new List<TileType>() { TileType.Floor };

        public bool IsMovable(TileType tile) => movableTiles.Contains(tile);
        public bool IsBlocking(TileType tile) => blockingTiles.Contains(tile);
        public bool IsWalkable(TileType tile) => walkableTiles.Contains(tile);

        private readonly static string[] floortiles = { "Floor0", "Floor1", "Floor2", "Floor3", "Floor4" };

        SpriteInstance sprite;

        public int Width { get; }

        public int Height { get; }

        public Rectangle Bounds => new Rectangle(0, 0, Width, Height);

        public Point TileSize { get; private set; }

        public Player Player { get; set; }

        public Tower Tower { get; set; }

        private Level(int width, int height)
        {
            Width = width;
            Height = height;
            data = new LevelTile[width, height];
            tileColors = new Dictionary<TileType, Color>()
            {
                [TileType.Floor] = Color.White,
                [TileType.Wall] = Color.DarkGray,
                [TileType.Movable] = Color.Green,
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
                    else if (data[x, y].Type == TileType.Tower)
                    {
                        if (Tower == null)
                            Tower = new Tower(x, y) { Level = this };
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
                        else if (level.data[x, y].Type == TileType.Tower)
                        {
                            if (level.Tower == null)
                                level.Tower = new Tower(x, y) { Level = level };
                        }
                    }
                    else
                        level.data[x, y] = new LevelTile() { Type = TileType.Floor, Frame = GameCore.Instance.Random.Next(0, floortiles.Length) };
                }
            }
            return level;
        }

        public bool TurnTile(Point start, Point end, Point pivot)
        {
            LevelTile startTile = GetTile(start);
            if (!IsMovable(startTile.Type))
                return false;

            LevelTile endTile = GetTile(end);
            if (!IsWalkable(endTile.Type))
                return false;

            Point p = start.X == pivot.X ? new Point(end.X, start.Y) : new Point(start.X, end.Y);
            LevelTile centerTile = GetTile(p);
            if (!IsWalkable(centerTile.Type))
                return false;

            data[start.X, start.Y].Type = TileType.Floor;
            data[end.X, end.Y].Type = TileType.Movable;
            return true;
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

            if (IsWalkable(GetTile(newPosition).Type))
            {
                data[position.X, position.Y].Type = TileType.Floor;
                data[newPosition.X, newPosition.Y].Type = TileType.Movable;

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

        public void DestroyTile(Point targetPosition)
        {
            LevelTile tile = GetTile(targetPosition);
            if (tile.Type == TileType.Destructible)
                data[targetPosition.X, targetPosition.Y].Type = TileType.Floor;
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

                        case TileType.Movable:
                            sprite.DrawFrame(new Vector2(x, y) * TileSize.ToVector2(), "Movable");
                            break;

                        case TileType.Destructible:
                            sprite.DrawFrame(new Vector2(x, y) * TileSize.ToVector2(), "BreakableWall");
                            break;
                    }
                }
            }

            Player?.Draw();
            Tower?.Draw();
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmtkJam2020.Gameplay
{
    public class Player
    {
        public Player(int x, int y)
        {
            Position = new Vector2(x, y);
        }

        public Vector2 Position { get; set; }

        public Orientation MoveDirection { get; private set; }

        public Level Level { get; set; }

        public void TurnClockwise()
        {
            switch (MoveDirection)
            {
                case Orientation.North:
                    MoveDirection = Orientation.East;
                    break;

                case Orientation.West:
                    MoveDirection = Orientation.North;
                    break;

                case Orientation.South:
                    MoveDirection = Orientation.West;
                    break;

                case Orientation.East:
                    MoveDirection = Orientation.South;
                    break;
            }
        }

        public void TurnCounterClockwise()
        {
            switch (MoveDirection)
            {
                case Orientation.North:
                    MoveDirection = Orientation.West;
                    break;

                case Orientation.West:
                    MoveDirection = Orientation.South;
                    break;

                case Orientation.South:
                    MoveDirection = Orientation.East;
                    break;

                case Orientation.East:
                    MoveDirection = Orientation.North;
                    break;
            }
        }

        public void MoveForward()
        {
            Vector2 newPosition = Position;
            switch (MoveDirection)
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

            MoveToPosition(newPosition);
        }

        private void MoveToPosition(Vector2 newPosition)
        {
            if (Level != null)
            {
                LevelTile levelTile = Level.GetTile(newPosition.ToPoint());
                if (levelTile.Type != TileType.Floor)
                    return;
            }

            Position = newPosition;
        }

        public void MoveBackward()
        {
            Vector2 newPosition = Position;
            switch (MoveDirection)
            {
                case Orientation.North:
                    newPosition.Y++;
                    break;

                case Orientation.West:
                    newPosition.X++;
                    break;

                case Orientation.South:
                    newPosition.Y--;
                    break;

                case Orientation.East:
                    newPosition.X--;
                    break;
            }

            MoveToPosition(newPosition);
        }

        public void Draw()
        {
            GameCore.Instance.SpriteBatch.Draw(GameCore.Instance.Pixel, new Rectangle((Position * new Vector2(Level.DEFAULT_TILE_WIDTH, Level.DEFAULT_TILE_HEIGHT)).ToPoint(), new Point(Level.DEFAULT_TILE_WIDTH, Level.DEFAULT_TILE_HEIGHT)), Color.Red);
        }
    }

    public enum Orientation
    {
        North,
        West,
        South,
        East
    }
}

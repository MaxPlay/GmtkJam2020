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
            Position = new Point(x, y);
        }

        public Point Position { get; set; }

        public Orientation MoveDirection { get; private set; }

        public Level Level { get; set; }

        public PlayerAction CurrentAction { get; set; }

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
            Point newPosition = Position;
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

            MoveToPosition(newPosition, MoveDirection, true);
        }

        private void MoveToPosition(Point newPosition, Orientation direction, bool forward)
        {
            if (Level != null)
            {
                LevelTile levelTile = Level.GetTile(newPosition);
                if (levelTile.Type != TileType.Floor)
                {
                    if (!forward)
                        return;

                    if (levelTile.Type == TileType.Pushable && CurrentAction == PlayerAction.Push)
                    {
                        if (!Level.PushTile(newPosition, direction))
                            return;
                    }
                    else
                        return;
                }
            }

            Position = newPosition;
        }

        public void MoveBackward()
        {
            Point newPosition = Position;
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

            MoveToPosition(newPosition, MoveDirection, false);
        }

        public void Draw()
        {
            GameCore.Instance.SpriteBatch.Draw(GameCore.Instance.Pixel, new Rectangle(Position.X * Level.DEFAULT_TILE_WIDTH, Position.Y * Level.DEFAULT_TILE_HEIGHT, Level.DEFAULT_TILE_WIDTH, Level.DEFAULT_TILE_HEIGHT), Color.Red);
        }
    }
}

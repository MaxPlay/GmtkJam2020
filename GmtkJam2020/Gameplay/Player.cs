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
        private Vector2 position;

        public Player(int x, int y)
        {
            Position = new Vector2(x, y);
        }

        public Vector2 Position { get => position; set => position = value; }

        public Orientation MoveDirection { get; private set; }

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
            switch (MoveDirection)
            {
                case Orientation.North:
                    position.Y--;
                    break;

                case Orientation.West:
                    position.X--;
                    break;

                case Orientation.South:
                    position.Y++;
                    break;

                case Orientation.East:
                    position.X++;
                    break;
            }
        }

        public void MoveBackward()
        {
            switch (MoveDirection)
            {
                case Orientation.North:
                    position.Y++;
                    break;

                case Orientation.West:
                    position.X++;
                    break;

                case Orientation.South:
                    position.Y--;
                    break;

                case Orientation.East:
                    position.X--;
                    break;
            }
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

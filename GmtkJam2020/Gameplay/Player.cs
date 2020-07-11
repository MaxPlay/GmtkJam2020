using GmtkJam2020.Rendering;
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
            sprite = SpriteManager.Sprites["Robot"].CreateInstance();
            sprite.Animator.SetAnimation(sprite.Source.NamedAnimations["Idle_Up"]);
        }

        public Point Position { get; set; }

        public Orientation MoveDirection { get; private set; }

        public Level Level { get; set; }

        public PlayerAction CurrentAction { get; set; }

        SpriteInstance sprite;

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

            UpdateAnimation();
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

            UpdateAnimation();
        }

        public void MoveForward() => MoveToPosition(GetPositionInFront(), MoveDirection, true);

        private Point GetPositionInFront()
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

            return newPosition;
        }

        public void MoveBackward() => MoveToPosition(GetPositionBehind(), MoveDirection, false);

        private Point GetPositionBehind()
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

            return newPosition;
        }

        private void MoveToPosition(Point newPosition, Orientation direction, bool forward)
        {
            bool canMove = true;
            if (Level != null)
            {
                LevelTile levelTile = Level.GetTile(newPosition);
                if (levelTile.Type == TileType.Floor)
                {
                    if (!forward)
                    {
                        Point positionInFront = GetPositionInFront();
                        LevelTile frontTile = Level.GetTile(positionInFront);
                        if (frontTile.Type == TileType.Moveable && (CurrentAction == PlayerAction.Pull || CurrentAction == PlayerAction.Grab))
                            Level.PushTile(positionInFront, (Orientation)((int)(direction + 2) % 4));
                    }
                }
                else
                {
                    if (forward)
                    {
                        if (levelTile.Type == TileType.Moveable && (CurrentAction == PlayerAction.Push || CurrentAction == PlayerAction.Grab))
                        {
                            if (!Level.PushTile(newPosition, direction))
                                canMove = false;
                        }
                        else
                            canMove = false;
                    }
                    else
                    {
                        canMove = false;
                    }
                }
            }

            if (canMove)
                Position = newPosition;
        }

        public void Destroy()
        {
            Point targetPosition = GetPositionInFront();

            Level.DestroyTile(targetPosition);
        }

        private void UpdateAnimation()
        {
            switch (MoveDirection)
            {
                case Orientation.North:
                    sprite.Animator.SetAnimation(sprite.Source.NamedAnimations["Idle_Up"]);
                    break;

                case Orientation.West:
                    sprite.Animator.SetAnimation(sprite.Source.NamedAnimations["Idle_Left"]);
                    break;

                case Orientation.South:
                    sprite.Animator.SetAnimation(sprite.Source.NamedAnimations["Idle_Down"]);
                    break;

                case Orientation.East:
                    sprite.Animator.SetAnimation(sprite.Source.NamedAnimations["Idle_Right"]);
                    break;
            }
        }

        public void Draw()
        {
            sprite.DrawAnimation(new Vector2(Position.X * Level.DEFAULT_TILE_WIDTH, Position.Y * Level.DEFAULT_TILE_HEIGHT));
        }
    }
}

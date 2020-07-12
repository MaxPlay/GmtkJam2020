using GmtkJam2020.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmtkJam2020.Gameplay
{
    public class Player : LevelEntity
    {
        Dictionary<PlayerAction, bool> actionAvailability = new Dictionary<PlayerAction, bool>()
        {
            [PlayerAction.Destroy] = false,
            [PlayerAction.Grab] = false,
            [PlayerAction.Move] = false,
            [PlayerAction.None] = false,
            [PlayerAction.Pull] = false,
            [PlayerAction.Push] = false,
            [PlayerAction.Turn] = false
        };

        public Player(int x, int y) : base(x, y)
        {
            sprite = SpriteManager.Sprites["Robot"].CreateInstance();
            sprite.Animator.SetAnimation(sprite.Source.NamedAnimations["Idle_Up"]);
        }

        public Orientation MoveDirection { get; private set; }

        public PlayerAction CurrentAction { get; set; }

        public void UpdateActionAvailability()
        {
            int distance = Level.GetTile(Position).Distance;
            actionAvailability[PlayerAction.Destroy] = Level.Tower.DestroyDistance > distance;
            actionAvailability[PlayerAction.Grab] = Level.Tower.GrabDistance > distance;
            actionAvailability[PlayerAction.Move] = Level.Tower.MoveDistance > distance;
            actionAvailability[PlayerAction.Pull] = Level.Tower.PullDistance > distance;
            actionAvailability[PlayerAction.Push] = Level.Tower.PushDistance > distance;
            actionAvailability[PlayerAction.Turn] = Level.Tower.TurnDistance > distance;
        }

        public bool IsActionAvailable(PlayerAction action) => actionAvailability[action];

        public void TurnClockwise()
        {
            if (CurrentAction == PlayerAction.Grab && IsActionAvailable(PlayerAction.Grab))
            {
                LevelTile grabTarget = Level.GetTile(GetPositionInFront());
                if (Level.IsMovable(grabTarget.Type))
                {
                    if (!Level.TurnTile(GetPositionInFront(), GetPositionOnRight(), Position))
                        return;
                }
            }

            if (!IsActionAvailable(PlayerAction.Turn))
                return;

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
            UpdateActionAvailability();
        }

        public void TurnCounterClockwise()
        {
            if (CurrentAction == PlayerAction.Grab && IsActionAvailable(PlayerAction.Grab))
            {
                LevelTile grabTarget = Level.GetTile(GetPositionInFront());
                if (Level.IsMovable(grabTarget.Type))
                {
                    if (!Level.TurnTile(GetPositionInFront(), GetPositionOnLeft(), Position))
                        return;
                }
            }

            if (!IsActionAvailable(PlayerAction.Turn))
                return;

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
            UpdateActionAvailability();
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

        private Point GetPositionOnLeft()
        {
            Point newPosition = Position;
            switch (MoveDirection)
            {
                case Orientation.North:
                    newPosition.X--;
                    break;

                case Orientation.West:
                    newPosition.Y++;
                    break;

                case Orientation.South:
                    newPosition.X++;
                    break;

                case Orientation.East:
                    newPosition.Y--;
                    break;
            }

            return newPosition;
        }

        private Point GetPositionOnRight()
        {
            Point newPosition = Position;
            switch (MoveDirection)
            {
                case Orientation.North:
                    newPosition.X++;
                    break;

                case Orientation.West:
                    newPosition.Y--;
                    break;

                case Orientation.South:
                    newPosition.X--;
                    break;

                case Orientation.East:
                    newPosition.Y++;
                    break;
            }

            return newPosition;
        }

        private void MoveToPosition(Point newPosition, Orientation direction, bool forward)
        {
            bool canMove = IsActionAvailable(PlayerAction.Move);
            if (!canMove)
                return;

            if (Level != null)
            {
                LevelTile levelTile = Level.GetTile(newPosition);
                if (Level.IsWalkable(levelTile.Type))
                {
                    if (!forward)
                    {
                        Point positionInFront = GetPositionInFront();
                        LevelTile frontTile = Level.GetTile(positionInFront);
                        if (Level.IsMovable(frontTile.Type) && (CurrentAction == PlayerAction.Pull && IsActionAvailable(PlayerAction.Pull) || CurrentAction == PlayerAction.Grab && IsActionAvailable(PlayerAction.Grab)))
                            Level.PushTile(positionInFront, (Orientation)((int)(direction + 2) % 4));
                    }
                }
                else
                {
                    if (forward)
                    {
                        if (Level.IsCollectible(levelTile.Type))
                        {
                            canMove = true;
                        }
                        else if (Level.IsMovable(levelTile.Type) && (CurrentAction == PlayerAction.Push && IsActionAvailable(PlayerAction.Push) || CurrentAction == PlayerAction.Grab && IsActionAvailable(PlayerAction.Grab)))
                        {
                            if (!Level.PushTile(newPosition, direction))
                                canMove = false;
                        }
                        else
                            canMove = false;
                    }
                    else
                    {
                        if (!Level.IsCollectible(levelTile.Type))
                            canMove = false;
                    }
                }
            }

            if (canMove)
            {
                Position = newPosition;
                UpdateActionAvailability();
                Level.Collect(Position);
            }
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

        public override void Draw()
        {
            sprite.DrawAnimation(new Vector2(Position.X * Level.DEFAULT_TILE_WIDTH, Position.Y * Level.DEFAULT_TILE_HEIGHT));
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmtkJam2020.Gameplay
{
    public class PlayerController : Controller
    {
        public Player Player { get; set; }

        readonly Dictionary<PlayerCommand, CommandDelegate> commands;

        public PlayerController()
        {
            commands = new Dictionary<PlayerCommand, CommandDelegate>()
            {
                [PlayerCommand.MoveDown] = () => IsKeyPressed(Keys.Down) || IsKeyPressed(Keys.S) || IsButtonPressed(Buttons.DPadDown) || IsDirection(Axis.Left, Direction.Down),
                [PlayerCommand.MoveUp] = () => IsKeyPressed(Keys.Up) || IsKeyPressed(Keys.W) || IsButtonPressed(Buttons.DPadUp) || IsDirection(Axis.Left, Direction.Up),
                [PlayerCommand.MoveLeft] = () => IsKeyPressed(Keys.Right) || IsKeyPressed(Keys.D) || IsButtonPressed(Buttons.DPadRight) || IsDirection(Axis.Left, Direction.Right),
                [PlayerCommand.MoveRight] = () => IsKeyPressed(Keys.Left) || IsKeyPressed(Keys.A) || IsButtonPressed(Buttons.DPadLeft) || IsDirection(Axis.Left, Direction.Left),
                [PlayerCommand.Push] = () => keyboardState.IsKeyDown(Keys.D1) || gamePadState.IsButtonDown(Buttons.A),
                [PlayerCommand.Pull] = () => keyboardState.IsKeyDown(Keys.D2) || gamePadState.IsButtonDown(Buttons.B),
                [PlayerCommand.Grab] = () => keyboardState.IsKeyDown(Keys.D3) || gamePadState.IsButtonDown(Buttons.X),
                [PlayerCommand.Destroy] = () => keyboardState.IsKeyDown(Keys.D4) || gamePadState.IsButtonDown(Buttons.Y),
            };
        }

        public override void Update()
        {
            base.Update();

            if (commands[PlayerCommand.MoveUp]())
                Player?.MoveForward();
            else if (commands[PlayerCommand.MoveDown]())
                Player?.MoveBackward();
            else if (commands[PlayerCommand.MoveRight]())
                Player?.TurnCounterClockwise();
            else if (commands[PlayerCommand.MoveLeft]())
                Player?.TurnClockwise();

            if (commands[PlayerCommand.Push]())
                Player.CurrentAction = PlayerAction.Push;
            else if (commands[PlayerCommand.Pull]())
                Player.CurrentAction = PlayerAction.Pull;
            else if (commands[PlayerCommand.Grab]())
                Player.CurrentAction = PlayerAction.Grab;
            else
                Player.CurrentAction = PlayerAction.None;

            if (commands[PlayerCommand.Destroy]())
                Player.Destroy();
        }

        private enum PlayerCommand
        {
            MoveUp,
            MoveRight,
            MoveDown,
            MoveLeft,
            Push,
            Pull,
            Grab,
            Destroy
        }

        private delegate bool CommandDelegate();
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmtkJam2020.Gameplay
{
    public class PlayerController
    {
        GamePadState gamePadState;
        KeyboardState keyboardState;

        public float DeadZone { get; set; }

        public Player Player { get; set; }

        readonly Dictionary<PlayerCommand, CommandDelegate> commands;

        public PlayerController()
        {
            commands = new Dictionary<PlayerCommand, CommandDelegate>()
            {
                [PlayerCommand.MoveDown] = (ref KeyboardState lastKeyboardState, ref GamePadState lastGamePadState) => IsKeyPressed(Keys.Down, ref keyboardState, ref lastKeyboardState) || IsKeyPressed(Keys.S, ref keyboardState, ref lastKeyboardState) || IsButtonPressed(Buttons.DPadDown, ref gamePadState, ref lastGamePadState) || IsDirection(Axis.Left, Direction.Down, ref gamePadState, ref lastGamePadState, DeadZone),
                [PlayerCommand.MoveUp] = (ref KeyboardState lastKeyboardState, ref GamePadState lastGamePadState) => IsKeyPressed(Keys.Up, ref keyboardState, ref lastKeyboardState) || IsKeyPressed(Keys.W, ref keyboardState, ref lastKeyboardState) || IsButtonPressed(Buttons.DPadUp, ref gamePadState, ref lastGamePadState) || IsDirection(Axis.Left, Direction.Up, ref gamePadState, ref lastGamePadState, DeadZone),
                [PlayerCommand.MoveLeft] = (ref KeyboardState lastKeyboardState, ref GamePadState lastGamePadState) => IsKeyPressed(Keys.Right, ref keyboardState, ref lastKeyboardState) || IsKeyPressed(Keys.D, ref keyboardState, ref lastKeyboardState) || IsButtonPressed(Buttons.DPadRight, ref gamePadState, ref lastGamePadState) || IsDirection(Axis.Left, Direction.Right, ref gamePadState, ref lastGamePadState, DeadZone),
                [PlayerCommand.MoveRight] = (ref KeyboardState lastKeyboardState, ref GamePadState lastGamePadState) => IsKeyPressed(Keys.Left, ref keyboardState, ref lastKeyboardState) || IsKeyPressed(Keys.A, ref keyboardState, ref lastKeyboardState) || IsButtonPressed(Buttons.DPadLeft, ref gamePadState, ref lastGamePadState) || IsDirection(Axis.Left, Direction.Left, ref gamePadState, ref lastGamePadState, DeadZone),
                [PlayerCommand.Push] = (ref KeyboardState lastKeyboardState, ref GamePadState lastGamePadState) => keyboardState.IsKeyDown(Keys.D1) || gamePadState.IsButtonDown(Buttons.A),
            };
        }

        public void Update()
        {
            GamePadState lastGamePadState = gamePadState;
            KeyboardState lastKeyboardState = keyboardState;

            gamePadState = GamePad.GetState(PlayerIndex.One);
            keyboardState = Keyboard.GetState();

            if (commands[PlayerCommand.MoveUp](ref lastKeyboardState, ref lastGamePadState))
                Player?.MoveForward();
            else if (commands[PlayerCommand.MoveDown](ref lastKeyboardState, ref lastGamePadState))
                Player?.MoveBackward();
            else if (commands[PlayerCommand.MoveRight](ref lastKeyboardState, ref lastGamePadState))
                Player?.TurnCounterClockwise();
            else if (commands[PlayerCommand.MoveLeft](ref lastKeyboardState, ref lastGamePadState))
                Player?.TurnClockwise();

            if (commands[PlayerCommand.Push](ref lastKeyboardState, ref lastGamePadState))
                Player.CurrentAction = PlayerAction.Push;
            else
                Player.CurrentAction = PlayerAction.None;
        }

        public static bool IsKeyPressed(Keys key, ref KeyboardState keyboardState, ref KeyboardState lastKeyboardState) => lastKeyboardState.IsKeyUp(key) && keyboardState.IsKeyDown(key);

        public static bool IsButtonPressed(Buttons button, ref GamePadState gamePadState, ref GamePadState lastGamePadState) => lastGamePadState.IsButtonUp(button) && gamePadState.IsButtonDown(button);

        public static bool IsDirection(Axis axis, Direction direction, ref GamePadState gamePadState, ref GamePadState lastGamePadState, float deadZone)
        {
            if (!gamePadState.IsConnected)
                return false;

            Vector2 axisValue = (axis == Axis.Left) ? gamePadState.ThumbSticks.Left : gamePadState.ThumbSticks.Right;
            Vector2 lastAxisValue = (axis == Axis.Left) ? lastGamePadState.ThumbSticks.Left : lastGamePadState.ThumbSticks.Right;
            float triggerValue = (axis == Axis.Left) ? gamePadState.Triggers.Left : gamePadState.Triggers.Right;
            float lastTriggerValue = (axis == Axis.Left) ? lastGamePadState.Triggers.Left : lastGamePadState.Triggers.Right;

            switch (direction)
            {
                case Direction.Up:
                    return axisValue.Y > deadZone && lastAxisValue.Y < deadZone;

                case Direction.Left:
                    return axisValue.X < -deadZone && lastAxisValue.X > -deadZone;

                case Direction.Down:
                    return axisValue.Y < -deadZone && lastAxisValue.Y > -deadZone;

                case Direction.Right:
                    return axisValue.X > deadZone && lastAxisValue.X < deadZone;

                case Direction.Trigger:
                    return triggerValue > deadZone && lastTriggerValue < deadZone;
            }

            return false;
        }

        public enum Direction
        {
            Up,
            Left,
            Down,
            Right,
            Trigger
        }

        public enum Axis
        {
            Left,
            Right
        }

        private enum PlayerCommand
        {
            MoveUp,
            MoveRight,
            MoveDown,
            MoveLeft,
            Push
        }

        private delegate bool CommandDelegate(ref KeyboardState lastKeyboardState, ref GamePadState lastGamePadState);
    }
}

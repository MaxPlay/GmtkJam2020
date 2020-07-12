using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GmtkJam2020
{
    public abstract class Controller
    {
        protected GamePadState gamePadState;
        protected KeyboardState keyboardState;

        protected GamePadState lastGamePadState;
        protected KeyboardState lastKeyboardState;

        public float DeadZone { get; set; }

        public virtual void Update()
        {
            lastGamePadState = gamePadState;
            lastKeyboardState = keyboardState;

            gamePadState = GamePad.GetState(PlayerIndex.One);
            keyboardState = Keyboard.GetState();
        }

        public bool IsKeyPressed(Keys key) => lastKeyboardState.IsKeyUp(key) && keyboardState.IsKeyDown(key);

        public bool IsButtonPressed(Buttons button) => lastGamePadState.IsButtonUp(button) && gamePadState.IsButtonDown(button);

        public bool IsDirection(Axis axis, Direction direction)
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
                    return axisValue.Y > DeadZone && lastAxisValue.Y < DeadZone && Math.Abs(axisValue.Y) > Math.Abs(axisValue.X);

                case Direction.Left:
                    return axisValue.X < -DeadZone && lastAxisValue.X > -DeadZone && Math.Abs(axisValue.X) > Math.Abs(axisValue.Y);

                case Direction.Down:
                    return axisValue.Y < -DeadZone && lastAxisValue.Y > -DeadZone && Math.Abs(axisValue.Y) > Math.Abs(axisValue.X);

                case Direction.Right:
                    return axisValue.X > DeadZone && lastAxisValue.X < DeadZone && Math.Abs(axisValue.X) > Math.Abs(axisValue.Y);

                case Direction.Trigger:
                    return triggerValue > DeadZone && lastTriggerValue < DeadZone;
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

    }
}
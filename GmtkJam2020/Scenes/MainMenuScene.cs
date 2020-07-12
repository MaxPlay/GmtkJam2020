using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GmtkJam2020.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GmtkJam2020.Scenes
{
    public class MainMenuScene : Scene
    {
        public override string Name => "MainMenu";

        public Button StartButton { get; set; }

        public Button LevelSelectButton { get; set; }

        public Button QuitButton { get; set; }

        public int SelectedIndex { get; set; }

        public MenuController MenuController { get; set; }

        private List<Button> buttons;

        public override void Draw(float deltaTime)
        {
            buttons.ForEach(b => b.Draw(SelectedIndex));
        }

        public override void Start()
        {
            StartButton = new Button() { Text = "Start", Index = 0, Bounds = new Rectangle(0, 0, 160, 20),  };
            LevelSelectButton = new Button() { Text = "Level Select", Index = 1, Bounds = new Rectangle(0, 40, 160, 20) };
            QuitButton = new Button() { Text = "Quit", Index = 2, Bounds = new Rectangle(0, 80, 160, 20) };
            buttons = new List<Button>
            {
                StartButton,
                LevelSelectButton,
                QuitButton
            };
            MenuController = new MenuController();
        }

        public override void Stop()
        {

        }

        public override void Update(float deltaTime)
        {
            if (MenuController.IsKeyPressed(Keys.Down) || MenuController.IsKeyPressed(Keys.S) || MenuController.IsButtonPressed(Buttons.DPadDown) || MenuController.IsDirection(Controller.Axis.Left, Controller.Direction.Down))
            {
                SelectedIndex++;
                if (SelectedIndex == buttons.Count)
                    SelectedIndex = 0;
            }

            if (MenuController.IsKeyPressed(Keys.Up) || MenuController.IsKeyPressed(Keys.W) || MenuController.IsButtonPressed(Buttons.DPadUp) || MenuController.IsDirection(Controller.Axis.Left, Controller.Direction.Up))
            {
                SelectedIndex--;
                if (SelectedIndex == -1)
                    SelectedIndex = buttons.Count - 1;
            }

            if (MenuController.IsKeyPressed(Keys.Enter) || MenuController.IsKeyPressed(Keys.Space) || MenuController.IsButtonPressed(Buttons.A))
            {
                buttons.Find(b => b.Index == SelectedIndex)?.Execute?.Invoke();
            }
        }
    }
}

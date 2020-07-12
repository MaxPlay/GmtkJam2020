using GmtkJam2020.Rendering;
using GmtkJam2020.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmtkJam2020.Scenes
{
    public class LevelSelectScene : Scene
    {
        public override string Name => "Level Select";

        public Button StartButton { get; set; }

        public Button NextLevelButton { get; set; }

        public Button PreviousLevelButton { get; set; }

        public Button BackButton { get; set; }
        public SpriteInstance Sprite { get; private set; }
        public int SelectedIndex { get; set; }

        public int SelectedLevel { get; set; }

        public MenuController MenuController { get; set; }

        private List<Button> buttons;

        private List<string> levels;

        public override void Draw(float deltaTime)
        {
            Sprite.DrawFrame(new Vector2(), "Default");
            buttons.ForEach(b => b.Draw(SelectedIndex));
        }

        public override void Start()
        {
            Sprite = SpriteManager.Sprites["TitleScreen"].CreateInstance();
            SelectedIndex = 1;
            PreviousLevelButton = new Button() { Text = "Previous Level", Index = 0, Bounds = new Rectangle(170, 75, 140, 25), Execute = PreviousLevel };
            StartButton = new Button() { Text = "Start", Index = 1, Bounds = new Rectangle(170, 100, 140, 25), Execute = StartGame };
            NextLevelButton = new Button() { Text = "Next Level", Index = 2, Bounds = new Rectangle(170, 125, 140, 25), Execute = NextLevel };
            BackButton = new Button() { Text = "Back", Index = 3, Bounds = new Rectangle(170, 150, 140, 25), Execute = Back };
            buttons = new List<Button>
            {
                StartButton,
                NextLevelButton,
                PreviousLevelButton,
                BackButton
            };
            MenuController = Manager.MenuController;
            levels = Manager.GetScene<GameScene>().LevelManager.Levels;
            UpdateStartButtonText();
        }

        private void UpdateStartButtonText()
        {
            StartButton.Text = $"Start {levels[SelectedLevel]}";
        }

        private void Back()
        {
            Manager.SetScene<MainMenuScene>();
        }

        private void PreviousLevel()
        {
            SelectedLevel--;
            if (SelectedLevel == -1)
                SelectedLevel = levels.Count - 1;
            UpdateStartButtonText();
        }

        private void NextLevel()
        {
            SelectedLevel++;
            if (SelectedLevel == levels.Count)
                SelectedLevel = 0;
            UpdateStartButtonText();
        }

        private void StartGame()
        {
            Manager.GetScene<GameScene>().SetLevel(SelectedLevel);
            Manager.SetScene<GameScene>();
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

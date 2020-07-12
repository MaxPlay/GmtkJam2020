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
    public class CompletionScene : Scene
    {
        public override string Name => "Completion";

        public SpriteInstance Sprite { get; private set; }

        public TextBlock TextBlock { get; set; }

        public Button MenuButton { get; set; }

        public int SelectedIndex { get; set; }

        public MenuController MenuController { get; set; }
        
        public override void Draw(float deltaTime)
        {
            Sprite.DrawFrame(new Vector2(), "Default");
            MenuButton.Draw(SelectedIndex);
            TextBlock.Draw();
        }

        public override void Start()
        {
            Sprite = SpriteManager.Sprites["TitleScreen"].CreateInstance();
            MenuButton = new Button() { Text = "Main Menu", Index = 0, Bounds = new Rectangle(170, 150, 140, 25), Execute = BackToMain };
            TextBlock = new TextBlock() { Bounds = new Rectangle(150, 80, 140, 40), Text = "Thank you for playing!" };
            MenuController = Manager.MenuController;
        }

        private void BackToMain()
        {
            Manager.SetScene<MainMenuScene>();
        }

        public override void Stop()
        {

        }

        public override void Update(float deltaTime)
        {
            if (MenuController.IsKeyPressed(Keys.Enter) || MenuController.IsKeyPressed(Keys.Space) || MenuController.IsButtonPressed(Buttons.A))
            {
                MenuButton?.Execute?.Invoke();
            }
        }
    }
}

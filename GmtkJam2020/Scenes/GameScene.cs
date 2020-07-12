using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GmtkJam2020.Gameplay;
using GmtkJam2020.UI;
using Microsoft.Xna.Framework;

namespace GmtkJam2020.Scenes
{
    public class GameScene : Scene
    {
        public override string Name => "Game";

        public LevelManager LevelManager { get; } = new LevelManager();

        private LevelUI levelUI;
        private Level level;
        private PlayerController controller;

        public override void Draw(float deltaTime)
        {
            level.Draw(deltaTime);
            levelUI.Draw();
        }

        public string CurrentLevel { get; set; }

        public override void Start()
        {
            levelUI = new LevelUI();
            level = Level.LoadFromFile($"Content/Levels/{CurrentLevel}.lvl");
            controller = new PlayerController() { Player = level.Player, DeadZone = 0.3f };
        }

        public override void Stop()
        {

        }

        public override void Update(float deltaTime)
        {
            controller.Update();
            level.Update(deltaTime);
            levelUI.Update(level.Player);
        }

        public void SetLevel(int selectedLevel)
        {
            if (selectedLevel >= 0 && selectedLevel < LevelManager.Levels.Count)
                CurrentLevel = LevelManager.Levels[selectedLevel];
        }
    }
}

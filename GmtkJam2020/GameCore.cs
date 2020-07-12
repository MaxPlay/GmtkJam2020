using GmtkJam2020.Gameplay;
using GmtkJam2020.Rendering;
using GmtkJam2020.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GmtkJam2020
{
    public class GameCore : Game
    {
        public static GameCore Instance { get; private set; }

        public GraphicsDeviceManager Graphics { get; private set; }

        public SpriteBatch SpriteBatch { get; private set; }

        public Texture2D Pixel { get; private set; }

        public Random Random { get; private set; }

        public bool DebugEnabled { get; set; }

        RenderTarget2D renderTarget;

        private Level level;

        private PlayerController Controller;

        private LevelUI levelUI;

        public GameCore()
        {
            Instance = this;
            Random = new Random();
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteManager.LoadSprite("MarsTiles");
            SpriteManager.LoadSprite("Robot");
            SpriteManager.LoadSprite("SignalTower");
            SpriteManager.LoadSprite("Diamond");
            SpriteManager.LoadSprite("LevelUI_Background");
            SpriteManager.LoadSprite("LevelUI_Icons");

            SpriteManager.LoadFont("Font");

            levelUI = new LevelUI();
            renderTarget = new RenderTarget2D(GraphicsDevice, 320, 180);
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Pixel = new Texture2D(GraphicsDevice, 1, 1);
            Pixel.SetData(new Color[] { Color.White });
            level = Level.LoadFromFile("Content/Levels/0.lvl");
            Controller = new PlayerController() { Player = level.Player, DeadZone = 0.3f };
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            {
                KeyboardState keyboardState = Keyboard.GetState();
                DebugEnabled = keyboardState.IsKeyDown(Keys.F3);
            }

            Controller.Update();
            levelUI.Update(level.Player);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);

            GraphicsDevice.Clear(Color.Black);
            SpriteBatch.Begin();
            level.Draw();
            levelUI.Draw();
            SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            SpriteBatch.Draw(renderTarget, new Rectangle(0, 0, GraphicsDeviceManager.DefaultBackBufferWidth, GraphicsDeviceManager.DefaultBackBufferHeight), Color.White);
            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

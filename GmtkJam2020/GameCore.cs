﻿using GmtkJam2020.Gameplay;
using GmtkJam2020.Rendering;
using GmtkJam2020.Scenes;
using GmtkJam2020.Sounds;
using GmtkJam2020.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.IO;

namespace GmtkJam2020
{
    public class GameCore : Game
    {
        public static GameCore Instance { get; private set; }

        public GraphicsDeviceManager Graphics { get; private set; }

        public SpriteBatch SpriteBatch { get; private set; }

        public Texture2D Pixel { get; private set; }

        public Random Random { get; private set; }

        public SceneManager SceneManager { get; set; }

        public bool DebugEnabled { get; set; }
        public Configuration Configuration { get; private set; }

        RenderTarget2D renderTarget;

        public GameCore()
        {
            Instance = this;
            Random = new Random();
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            SceneManager = new SceneManager();
            LoadScreenConfig();
        }

        private void LoadScreenConfig()
        {
            if (File.Exists("config.json"))
            {
                Configuration = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText("config.json"));
            }
            else
            {
                Configuration = Configuration.CreateDefault();
                File.WriteAllText("config.json", JsonConvert.SerializeObject(Configuration));
            }
        }

        protected override void Initialize()
        {
            Graphics.PreferredBackBufferWidth = Configuration.Width;
            Graphics.PreferredBackBufferHeight = Configuration.Height;
            Graphics.IsFullScreen = Configuration.Fullscreen;
            IsMouseVisible = !Configuration.Fullscreen;
            Graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteManager.LoadSprite("TitleScreen");
            SpriteManager.LoadSprite("MarsTiles");
            SpriteManager.LoadSprite("Robot");
            SpriteManager.LoadSprite("SignalTower");
            SpriteManager.LoadSprite("Diamond");
            SpriteManager.LoadSprite("LevelUI_Background");
            SpriteManager.LoadSprite("LevelUI_Icons");

            SoundManager.LoadFiles();
            SoundManager.SetVolume(0.1f);

            SpriteManager.LoadFont("Font");
            SpriteManager.LoadFont("MenuFont");

            renderTarget = new RenderTarget2D(GraphicsDevice, 320, 180);
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Pixel = new Texture2D(GraphicsDevice, 1, 1);
            Pixel.SetData(new Color[] { Color.White });

            new SplashScreen();
            new LevelSelectScene();
            new MainMenuScene();
            new GameScene();
            new CompletionScene();

            SceneManager.SetScene<MainMenuScene>();
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
            SceneManager.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);

            GraphicsDevice.Clear(Color.Black);
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
            SceneManager.Draw((float)gameTime.ElapsedGameTime.TotalSeconds);
            SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp);
            SpriteBatch.Draw(renderTarget, new Rectangle(0, 0, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight), Color.White);
            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

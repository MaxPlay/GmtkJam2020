using Microsoft.Xna.Framework;

namespace GmtkJam2020.Scenes
{
    public abstract class Scene
    {
        public Scene()
        {
            GameCore.Instance.SceneManager.RegisterScene(this);
            Manager = GameCore.Instance.SceneManager;
        }

        public SceneManager Manager { get; }

        public abstract string Name { get; }

        public abstract void Start();

        public abstract void Update(float deltaTime);

        public abstract void Stop();

        public abstract void Draw(float deltaTime);
    }
}
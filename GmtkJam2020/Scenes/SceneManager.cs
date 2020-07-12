using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmtkJam2020.Scenes
{
    public class SceneManager
    {
        public List<Scene> Scenes { get; } = new List<Scene>();

        public Dictionary<Type, Scene> NamedScenes { get; } = new Dictionary<Type, Scene>();

        public Scene CurrentScene { get; private set; }

        public T GetScene<T>() where T : Scene => NamedScenes[typeof(T)] as T;

        public void RegisterScene(Scene scene)
        {
            if (!NamedScenes.ContainsKey(scene.GetType()))
            {
                NamedScenes.Add(scene.GetType(), scene);
                Scenes.Add(scene);
            }
        }

        public void SetScene<T>() where T : Scene
        {
            if (NamedScenes.ContainsKey(typeof(T)))
            {
                CurrentScene?.Stop();
                CurrentScene = NamedScenes[typeof(T)];
                CurrentScene.Start();
            }
        }

        public void Update(float deltaTime) => CurrentScene?.Update(deltaTime);

        public void Draw(float deltaTime) => CurrentScene?.Draw(deltaTime);
    }
}

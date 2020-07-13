namespace GmtkJam2020
{
    public class Configuration
    {
        public int Width { get; set; } = 640;

        public int Height { get; set; } = 360;

        public bool Fullscreen { get; set; } = false;
        
        public static Configuration CreateDefault()
        {
            return new Configuration()
            {
                Width = 1920,
                Height = 1080,
                Fullscreen = true
            };
        }
    }
}
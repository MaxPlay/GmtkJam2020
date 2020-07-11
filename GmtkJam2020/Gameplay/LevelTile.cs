namespace GmtkJam2020.Gameplay
{
    public struct LevelTile
    {
        public TileType Type;

        public static LevelTile OutOfBoundsTile => new LevelTile() { Type = TileType.Wall };
    }
}
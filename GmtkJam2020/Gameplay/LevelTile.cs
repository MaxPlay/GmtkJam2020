namespace GmtkJam2020.Gameplay
{
    public struct LevelTile
    {
        public TileType Type;

        public int Frame;

        public int Distance;

        public static LevelTile OutOfBoundsTile => new LevelTile() { Type = TileType.Wall };
    }
}
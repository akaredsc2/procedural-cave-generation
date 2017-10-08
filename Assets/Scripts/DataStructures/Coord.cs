namespace DataStructures {
    public struct Coord {
        private readonly int tileX;
        private readonly int tileY;

        public Coord(int tileX, int tileY) {
            this.tileX = tileX;
            this.tileY = tileY;
        }

        public int TileX {
            get { return tileX; }
        }

        public int TileY {
            get { return tileY; }
        }
    }
}

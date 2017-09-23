namespace DataStructures
{
    public class MapProperties
    {
        private readonly int width;
        private readonly int height;
        private readonly string seed;
        private readonly bool useRandomSeed;
        private readonly int randomFillPercent;
        private readonly int smoothingIterationCount;
        private readonly float squareSize;

        public MapProperties(int width, int height, string seed, bool useRandomSeed,
            int randomFillPercent, int smoothingIterationCount, float squareSize)
        {
            this.width = width;
            this.height = height;
            this.seed = seed;
            this.useRandomSeed = useRandomSeed;
            this.randomFillPercent = randomFillPercent;
            this.smoothingIterationCount = smoothingIterationCount;
            this.squareSize = squareSize;
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public string Seed
        {
            get { return seed; }
        }

        public bool UseRandomSeed
        {
            get { return useRandomSeed; }
        }

        public int RandomFillPercent
        {
            get { return randomFillPercent; }
        }

        public int SmoothingIterationCount
        {
            get { return smoothingIterationCount; }
        }

        public float SquareSize
        {
            get { return squareSize; }
        }
    }
}
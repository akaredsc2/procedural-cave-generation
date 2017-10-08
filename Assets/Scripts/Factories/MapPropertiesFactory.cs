using DataStructures;

namespace Factories {
    public class MapPropertiesFactory {
        public MapProperties createMapProperties(int width, int height, string seed, bool useRandomSeed,
                                                 int randomFillPercent, int smoothingIterationCount, float squareSize) {
            return new MapProperties(width, height, seed, useRandomSeed, randomFillPercent, smoothingIterationCount,
                                     squareSize);
        }
    }
}
using UnityEngine;

namespace Factories
{
    public class ProceduralCaveGenerationAbstractFactory : MonoBehaviour
    {
        private readonly SquareGridFactory squareGridFactory = new SquareGridFactory();
        private readonly MapFactory mapFactory = new MapFactory();
        private readonly MapPropertiesFactory mapPropertiesFactory = new MapPropertiesFactory();

        public SquareGridFactory SquareGridFactory
        {
            get { return squareGridFactory; }
        }

        public MapFactory MapFactory
        {
            get { return mapFactory; }
        }

        public MapPropertiesFactory MapPropertiesFactory
        {
            get { return mapPropertiesFactory; }
        }
    }
}
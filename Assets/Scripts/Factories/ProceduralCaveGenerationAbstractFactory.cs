using UnityEngine;

namespace Factories
{
    public class ProceduralCaveGenerationAbstractFactory : MonoBehaviour
    {
        private readonly SquareFactory squareFactory;
        private readonly SquareGridFactory squareGridFactory;
        private readonly MapFactory mapFactory;
        private readonly MapPropertiesFactory mapPropertiesFactory;

        public ProceduralCaveGenerationAbstractFactory()
        {
            squareFactory = new SquareFactory();
            squareGridFactory = new SquareGridFactory(squareFactory);
            mapFactory = new MapFactory();
            mapPropertiesFactory = new MapPropertiesFactory();
        }

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

        public SquareFactory SquareFactory
        {
            get { return squareFactory; }
        }
    }
}
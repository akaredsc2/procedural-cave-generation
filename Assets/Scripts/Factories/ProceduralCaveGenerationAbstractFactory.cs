using UnityEngine;

namespace Factories {
    public class ProceduralCaveGenerationAbstractFactory : MonoBehaviour {
        private readonly SquareFactory squareFactory;
        private readonly SquareGridFactory squareGridFactory;
        private readonly MapFactory mapFactory;
        private readonly MapPropertiesFactory mapPropertiesFactory;
        private readonly MeshFactory meshFactory;

        public ProceduralCaveGenerationAbstractFactory() {
            squareFactory = new SquareFactory();
            squareGridFactory = new SquareGridFactory(squareFactory);
            mapFactory = new MapFactory();
            mapPropertiesFactory = new MapPropertiesFactory();
            meshFactory = new MeshFactory();
        }

        public SquareGridFactory SquareGridFactory {
            get { return squareGridFactory; }
        }

        public MapFactory MapFactory {
            get { return mapFactory; }
        }

        public MapPropertiesFactory MapPropertiesFactory {
            get { return mapPropertiesFactory; }
        }

        public SquareFactory SquareFactory {
            get { return squareFactory; }
        }

        public MeshFactory MeshFactory {
            get { return meshFactory; }
        }
    }
}
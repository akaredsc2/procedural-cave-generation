using DataStructures;
using Factories;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public ProceduralCaveGenerationAbstractFactory proceduralCaveGenerationAbstractFactory;

    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;

    [Range(0, 100)] public int randomFillPercent;

    public int smoothingIterationCount;
    public float squareSize;

    public int borderSize;

    private MapFactory mapFactory;
    private MapPropertiesFactory mapPropertiesFactory;
    private int[,] map;

    private void Start()
    {
        mapFactory = proceduralCaveGenerationAbstractFactory.MapFactory;
        mapPropertiesFactory = proceduralCaveGenerationAbstractFactory.MapPropertiesFactory;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MapProperties mapProperties = mapPropertiesFactory.createMapProperties(width, height, seed, useRandomSeed,
                randomFillPercent, smoothingIterationCount, squareSize);

            map = mapFactory.CreateMap(mapProperties);

            // todo move to map factory
            int[,] borderedMap = new int[width + borderSize * 2, height + borderSize * 2];

            for (int i = 0; i < borderedMap.GetLength(0); i++)
            {
                for (int j = 0; j < borderedMap.GetLength(1); j++)
                {
                    if (i >= borderSize && i < width + borderSize && j >= borderSize && j < height + borderSize)
                    {
                        borderedMap[i, j] = map[i - borderSize, j - borderSize];
                    }
                    else
                    {
                        borderedMap[i, j] = 1;
                    }
                }
            }

            MeshGenerator meshGenerator = GetComponent<MeshGenerator>();
            meshGenerator.GenerateMesh(borderedMap, 1);
        }
    }
}
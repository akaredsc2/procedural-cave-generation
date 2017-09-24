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
            
            MeshGenerator meshGenerator = GetComponent<MeshGenerator>();
            meshGenerator.GenerateMesh(map, 1);
        }
    }

    private void OnDrawGizmos()
    {
        if (map != null)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Gizmos.color = map[i, j] == 1 ? Color.black : Color.white;
                    Vector3 position = new Vector3(-width / 2 + i + 0.5f, 0, -height / 2 + j + 0.5f);
                    Gizmos.DrawCube(position, Vector3.one);
                }
            }
        }
    }
}
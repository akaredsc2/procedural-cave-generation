using DataStructures;
using Factories;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public ProceduralCaveGenerationAbstractFactory proceduralCaveGenerationAbstractFactory;

    public MeshFilter wallsMeshFilter;
    public MeshFilter cave;

    private SquareGridFactory squareGridFactory;
    private MeshFactory meshFactory;

    public bool is2D;

    private void Start()
    {
        squareGridFactory = proceduralCaveGenerationAbstractFactory.SquareGridFactory;
        meshFactory = proceduralCaveGenerationAbstractFactory.MeshFactory;
    }

    public void GenerateMesh(int[,] map, float squareSize)
    {
        SquareGrid squareGrid = squareGridFactory.CreateSqaureGrid(map, squareSize);
        cave.mesh = meshFactory.CreateMesh(squareGrid, wallsMeshFilter, is2D, gameObject, squareSize);
    }
}
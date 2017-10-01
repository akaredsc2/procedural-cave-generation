using DataStructures;
using Factories;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public ProceduralCaveGenerationAbstractFactory proceduralCaveGenerationAbstractFactory;

    public MeshFilter wallsMeshFilter;

    private SquareGridFactory squareGridFactory;
    private MeshFactory meshFactory;

    private void Start()
    {
        squareGridFactory = proceduralCaveGenerationAbstractFactory.SquareGridFactory;
        meshFactory = proceduralCaveGenerationAbstractFactory.MeshFactory;
    }

    public void GenerateMesh(int[,] map, float squareSize)
    {
        SquareGrid squareGrid = squareGridFactory.CreateSqaureGrid(map, squareSize);
        GetComponent<MeshFilter>().mesh = meshFactory.CreateMesh(squareGrid, wallsMeshFilter);
    }
}
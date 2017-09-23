using DataStructures;
using Factories;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public ProceduralCaveGenerationAbstractFactory proceduralCaveGenerationAbstractFactory;

    private SquareGridFactory squareGridFactory;
    private SquareGrid squareGrid;

    private void Start()
    {
        squareGridFactory = proceduralCaveGenerationAbstractFactory.SquareGridFactory;
    }

    public void GenerateMesh(int[,] map, float squareSize)
    {
        squareGrid = squareGridFactory.CreateSqaureGrid(map, squareSize);
    }

    private void OnDrawGizmos()
    {
        if (squareGrid != null)
        {
            for (int i = 0; i < squareGrid.Squares.GetLength(0); i++)
            {
                for (int j = 0; j < squareGrid.Squares.GetLength(1); j++)
                {
                    Gizmos.color = squareGrid.Squares[i, j].TopLeft.IsActive ? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.Squares[i, j].TopLeft.Position, Vector3.one * 0.4f);

                    Gizmos.color = squareGrid.Squares[i, j].TopRight.IsActive ? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.Squares[i, j].TopRight.Position, Vector3.one * 0.4f);

                    Gizmos.color = squareGrid.Squares[i, j].BottomRight.IsActive ? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.Squares[i, j].BottomRight.Position, Vector3.one * 0.4f);

                    Gizmos.color = squareGrid.Squares[i, j].BottomLeft.IsActive ? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.Squares[i, j].BottomLeft.Position, Vector3.one * 0.4f);

                    Gizmos.color = Color.gray;
                    Gizmos.DrawCube(squareGrid.Squares[i, j].CenterTop.Position, Vector3.one * 0.15f);
                    Gizmos.DrawCube(squareGrid.Squares[i, j].CenterRight.Position, Vector3.one * 0.15f);
                    Gizmos.DrawCube(squareGrid.Squares[i, j].CenterBottom.Position, Vector3.one * 0.15f);
                    Gizmos.DrawCube(squareGrid.Squares[i, j].CenterLeft.Position, Vector3.one * 0.15f);
                }
            }
        }
    }
}
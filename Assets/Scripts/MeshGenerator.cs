using System.Collections.Generic;
using DataStructures;
using Factories;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public ProceduralCaveGenerationAbstractFactory proceduralCaveGenerationAbstractFactory;

    private SquareGridFactory squareGridFactory;
    private SquareGrid squareGrid;

    private List<Vector3> vertices;
    private List<int> triangleIndecies;

    private void Start()
    {
        squareGridFactory = proceduralCaveGenerationAbstractFactory.SquareGridFactory;
    }

    public void GenerateMesh(int[,] map, float squareSize)
    {
        squareGrid = squareGridFactory.CreateSqaureGrid(map, squareSize);

        vertices = new List<Vector3>();
        triangleIndecies = new List<int>();
        
        for (int i = 0; i < squareGrid.Squares.GetLength(0); i++)
        {
            for (int j = 0; j < squareGrid.Squares.GetLength(1); j++)
            {
                TriangulateSquare(squareGrid.Squares[i, j]);
            }
        }
        
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangleIndecies.ToArray();
        mesh.RecalculateNormals();
    }

    void TriangulateSquare(Square square)
    {
        switch (square.Configuration)
        {
            case 0:
                break;

            // 1 active control node
            case 1:
                MeshFromPoints(square.CenterBottom, square.BottomLeft, square.CenterLeft);
                break;
            case 2:
                MeshFromPoints(square.CenterRight, square.BottomRight, square.CenterBottom);
                break;
            case 4:
                MeshFromPoints(square.CenterTop, square.TopRight, square.CenterRight);
                break;
            case 8:
                MeshFromPoints(square.TopLeft, square.CenterTop, square.CenterLeft);
                break;

            // 2 active control nodes
            case 3:
                MeshFromPoints(square.CenterRight, square.BottomRight, square.BottomLeft, square.CenterLeft);
                break;
            case 6:
                MeshFromPoints(square.CenterTop, square.TopRight, square.BottomRight, square.CenterBottom);
                break;
            case 9:
                MeshFromPoints(square.TopLeft, square.CenterTop, square.CenterBottom, square.BottomLeft);
                break;
            case 12:
                MeshFromPoints(square.TopLeft, square.TopRight, square.CenterRight, square.CenterLeft);
                break;
            case 5:
                MeshFromPoints(square.CenterTop, square.TopRight, square.CenterRight, square.CenterBottom,
                    square.BottomLeft, square.CenterLeft);
                break;
            case 10:
                MeshFromPoints(square.TopLeft, square.CenterTop, square.CenterRight, square.BottomRight,
                    square.CenterBottom, square.CenterLeft);
                break;

            // 3 active control nodes
            case 7:
                MeshFromPoints(square.CenterTop, square.TopRight, square.BottomRight, square.BottomLeft,
                    square.CenterLeft);
                break;
            case 11:
                MeshFromPoints(square.TopLeft, square.CenterTop, square.CenterRight, square.BottomRight,
                    square.BottomLeft);
                break;
            case 13:
                MeshFromPoints(square.TopLeft, square.TopRight, square.CenterRight, square.CenterBottom,
                    square.BottomLeft);
                break;
            case 14:
                MeshFromPoints(square.TopLeft, square.TopRight, square.BottomRight, square.CenterBottom,
                    square.CenterLeft);
                break;

            // 4 active control nodes
            case 15:
                MeshFromPoints(square.TopLeft, square.TopRight, square.BottomRight, square.BottomLeft);
                break;
        }
    }

    void MeshFromPoints(params Node[] points)
    {
        AssignVertices(points);

        if (points.Length >= 3)
            CreateTriangle(points[0], points[1], points[2]);
        if (points.Length >= 4)
            CreateTriangle(points[0], points[2], points[3]);
        if (points.Length >= 5)
            CreateTriangle(points[0], points[3], points[4]);
        if (points.Length >= 6)
            CreateTriangle(points[0], points[4], points[5]);
    }

    void AssignVertices(Node[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].VertexIndex == -1)
            {
                //todo refactor to create new point with correct vertex index
                points[i].VertexIndex = vertices.Count;
                vertices.Add(points[i].Position);
            }
        }
    }

    void CreateTriangle(Node a, Node b, Node c)
    {
        triangleIndecies.Add(a.VertexIndex);
        triangleIndecies.Add(b.VertexIndex);
        triangleIndecies.Add(c.VertexIndex);
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
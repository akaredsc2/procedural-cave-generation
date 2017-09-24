using System.Collections.Generic;
using DataStructures;
using UnityEngine;

namespace Factories
{
    public class MeshFactory
    {
        public Mesh CreateMesh(SquareGrid squareGrid)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> indecies = new List<int>();

            for (int i = 0; i < squareGrid.Squares.GetLength(0); i++)
            {
                for (int j = 0; j < squareGrid.Squares.GetLength(1); j++)
                {
                    TriangulateSquare(vertices, indecies, squareGrid.Squares[i, j]);
                }
            }

            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = indecies.ToArray();
            mesh.RecalculateNormals();
            
            return mesh;
        }

        private void TriangulateSquare(List<Vector3> vertices, List<int> indecies, Square square)
        {
            switch (square.Configuration)
            {
                case 0:
                    break;

                // 1 active control node
                case 1:
                    MeshFromPoints(vertices, indecies, square.CenterBottom, square.BottomLeft, square.CenterLeft);
                    break;
                case 2:
                    MeshFromPoints(vertices, indecies, square.CenterRight, square.BottomRight, square.CenterBottom);
                    break;
                case 4:
                    MeshFromPoints(vertices, indecies, square.CenterTop, square.TopRight, square.CenterRight);
                    break;
                case 8:
                    MeshFromPoints(vertices, indecies, square.TopLeft, square.CenterTop, square.CenterLeft);
                    break;

                // 2 active control nodes
                case 3:
                    MeshFromPoints(vertices, indecies, square.CenterRight, square.BottomRight, square.BottomLeft,
                        square.CenterLeft);
                    break;
                case 6:
                    MeshFromPoints(vertices, indecies, square.CenterTop, square.TopRight, square.BottomRight,
                        square.CenterBottom);
                    break;
                case 9:
                    MeshFromPoints(vertices, indecies, square.TopLeft, square.CenterTop, square.CenterBottom,
                        square.BottomLeft);
                    break;
                case 12:
                    MeshFromPoints(vertices, indecies, square.TopLeft, square.TopRight, square.CenterRight,
                        square.CenterLeft);
                    break;
                case 5:
                    MeshFromPoints(vertices, indecies, square.CenterTop, square.TopRight, square.CenterRight,
                        square.CenterBottom,
                        square.BottomLeft, square.CenterLeft);
                    break;
                case 10:
                    MeshFromPoints(vertices, indecies, square.TopLeft, square.CenterTop, square.CenterRight,
                        square.BottomRight,
                        square.CenterBottom, square.CenterLeft);
                    break;

                // 3 active control nodes
                case 7:
                    MeshFromPoints(vertices, indecies, square.CenterTop, square.TopRight, square.BottomRight,
                        square.BottomLeft,
                        square.CenterLeft);
                    break;
                case 11:
                    MeshFromPoints(vertices, indecies, square.TopLeft, square.CenterTop, square.CenterRight,
                        square.BottomRight,
                        square.BottomLeft);
                    break;
                case 13:
                    MeshFromPoints(vertices, indecies, square.TopLeft, square.TopRight, square.CenterRight,
                        square.CenterBottom,
                        square.BottomLeft);
                    break;
                case 14:
                    MeshFromPoints(vertices, indecies, square.TopLeft, square.TopRight, square.BottomRight,
                        square.CenterBottom,
                        square.CenterLeft);
                    break;

                // 4 active control nodes
                case 15:
                    MeshFromPoints(vertices, indecies, square.TopLeft, square.TopRight, square.BottomRight,
                        square.BottomLeft);
                    break;
            }
        }

        private void MeshFromPoints(List<Vector3> vertices, List<int> indecies, params Node[] points)
        {
            AssignVertices(vertices, points);

            if (points.Length >= 3)
                CreateTriangle(indecies, points[0], points[1], points[2]);
            if (points.Length >= 4)
                CreateTriangle(indecies, points[0], points[2], points[3]);
            if (points.Length >= 5)
                CreateTriangle(indecies, points[0], points[3], points[4]);
            if (points.Length >= 6)
                CreateTriangle(indecies, points[0], points[4], points[5]);
        }

        private void AssignVertices(List<Vector3> vertices, Node[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].VertexIndex == -1)
                {
                    points[i] = points[i].CopyWithVertexIndex(vertices.Count);
                    vertices.Add(points[i].Position);
                }
            }
        }

        private void CreateTriangle(List<int> indecies, Node a, Node b, Node c)
        {
            indecies.Add(a.VertexIndex);
            indecies.Add(b.VertexIndex);
            indecies.Add(c.VertexIndex);
        }
    }
}